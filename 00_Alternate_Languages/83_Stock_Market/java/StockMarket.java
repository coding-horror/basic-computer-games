import java.util.ArrayList;
import java.util.InputMismatchException;
import java.util.List;
import java.util.Random;
import java.util.Scanner;

/**
 * Stock Market Simulation
 *
 * Some of the original program's variables' documentation and their equivalent in this program:
 * A-MRKT TRND SLP;             marketTrendSlope
 * B5-BRKRGE FEE;               brokerageFee
 * C-TTL CSH ASSTS;             cashAssets
 * C5-TTL CSH ASSTS (TEMP);     tmpCashAssets
 * C(I)-CHNG IN STK VAL;        changeStockValue
 * D-TTL ASSTS;                 assets
 * E1,E2-LRG CHNG MISC;         largeChange1, largeChange2
 * I1,I2-STCKS W LRG CHNG;      randomStockIndex1, randomStockIndex2
 * N1,N2-LRG CHNG DAY CNTS;     largeChangeNumberDays1, largeChangeNumberDays2
 * P5-TTL DAYS PRCHSS;          totalDaysPurchases
 * P(I)-PRTFL CNTNTS;           portfolioContents
 * Q9-NEW CYCL?;                newCycle
 * S4-SGN OF A;                 slopeSign
 * S5-TTL DYS SLS;              totalDaysSales
 * S(I)-VALUE/SHR;              stockValue
 * T-TTL STCK ASSTS;            totalStockAssets
 * T5-TTL VAL OF TRNSCTNS;      totalValueOfTransactions
 * W3-LRG CHNG;                 bigChange
 * X1-SMLL CHNG(<$1);           smallChange
 * Z4,Z5,Z6-NYSE AVE.;          tmpNyseAverage, nyseAverage, nyseAverageChange
 * Z(I)-TRNSCT                  transactionQuantity
 *
 * new price = old price + (trend x old price) + (small random price
 * change) + (possible large price change)
 *
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class StockMarket {

	private static final Random random = new Random();

	public static void main(String[] args) {

		Scanner scan = new Scanner(System.in);

		printIntro();
		printGameHelp(scan);

		final List<Stock> stocks = initStocks();

		double marketTrendSlope = Math.floor((random.nextFloat() / 10) * 100 + 0.5)/100f;
		double totalValueOfTransactions;
		int largeChangeNumberDays1 = 0;
		int largeChangeNumberDays2 = 0;

		//DAYS FOR FIRST TREND SLOPE (A)
		var t8 = randomNumber(1, 6);

		//RANDOMIZE SIGN OF FIRST TREND SLOPE (A)
		if (random.nextFloat() <= 0.5) {
			marketTrendSlope = -marketTrendSlope;
		}

		// INITIALIZE CASH ASSETS:C
		double cashAssets = 10000;
		boolean largeChange1 = false;
		boolean largeChange2 = false;
		double tmpNyseAverage;
		double nyseAverage = 0;
		boolean inProgress = true;
		var firstRound = true;

		while (inProgress) {

			/* Original documentation:
			RANDOMLY PRODUCE NEW STOCK VALUES BASED ON PREVIOUS DAY'S VALUES
			N1,N2 ARE RANDOM NUMBERS OF DAYS WHICH RESPECTIVELY
			DETERMINE WHEN STOCK I1 WILL INCREASE 10 PTS. AND STOCK
			I2 WILL DECREASE 10 PTS.
			IF N1 DAYS HAVE PASSED, PICK AN I1, SET E1, DETERMINE NEW N1
			*/
			int randomStockIndex1 = 0;
			int randomStockIndex2 = 0;

			if (largeChangeNumberDays1 <= 0) {
				randomStockIndex1 = randomNumber(0, stocks.size());
				largeChangeNumberDays1 = randomNumber(1, 6);
				largeChange1 = true;
			}
			if (largeChangeNumberDays2 <= 0) {
				randomStockIndex2 = randomNumber(0, stocks.size());
				largeChangeNumberDays2 = randomNumber(1, 6);
				largeChange2 = true;
			}
			adjustAllStockValues(stocks, largeChange1, largeChange2, marketTrendSlope, stocks.get(randomStockIndex1), stocks.get(randomStockIndex2));

			//reset largeChange flags
			largeChange1 = false;
			largeChange2 = false;
			largeChangeNumberDays1--;
			largeChangeNumberDays2--;

			//AFTER T8 DAYS RANDOMLY CHANGE TREND SIGN AND SLOPE
			t8 = t8 - 1;
			if (t8 < 1) {
				marketTrendSlope = newMarketTrendSlope();
				t8 = randomNumber(1, 6);
			}

			//PRINT PORTFOLIO
			printPortfolio(firstRound, stocks);

			tmpNyseAverage = nyseAverage;
			nyseAverage = 0;
			double totalStockAssets = 0;
			for (Stock stock : stocks) {
				nyseAverage = nyseAverage + stock.getStockValue();
				totalStockAssets = totalStockAssets + stock.getStockValue() * stock.getPortfolioContents();
			}
			nyseAverage = Math.floor(100 * (nyseAverage / 5) + .5) / 100f;
			double nyseAverageChange = Math.floor((nyseAverage - tmpNyseAverage) * 100 + .5) / 100f;

			// TOTAL ASSETS:D
			double assets = totalStockAssets + cashAssets;
			if (firstRound) {
				System.out.printf("\n\nNEW YORK STOCK EXCHANGE AVERAGE: %.2f", nyseAverage);
			} else {
				System.out.printf("\n\nNEW YORK STOCK EXCHANGE AVERAGE: %.2f NET CHANGE %.2f", nyseAverage, nyseAverageChange);
			}

			totalStockAssets = Math.floor(100 * totalStockAssets + 0.5) / 100d;
			System.out.printf("\n\nTOTAL STOCK ASSETS ARE   $ %.2f", totalStockAssets);
       		cashAssets = Math.floor(100 * cashAssets + 0.5) / 100d;
			System.out.printf("\nTOTAL CASH ASSETS ARE    $ %.2f", cashAssets);
			assets = Math.floor(100 * assets + .5) / 100d;
			System.out.printf("\nTOTAL ASSETS ARE         $ %.2f\n", assets);

			if (!firstRound) {
				System.out.print("\nDO YOU WISH TO CONTINUE (YES-TYPE 1, NO-TYPE 0)? ");
				var newCycle = readANumber(scan);
				if (newCycle < 1) {
					System.out.println("HOPE YOU HAD FUN!!");
					inProgress = false;
				}
			}

			if (inProgress) {
				boolean validTransaction = false;
				//    TOTAL DAY'S PURCHASES IN $:P5
				double totalDaysPurchases = 0;
				//    TOTAL DAY'S SALES IN $:S5
				double totalDaysSales = 0;
				double tmpCashAssets;
				while (!validTransaction) {
					//INPUT TRANSACTIONS
					readStockTransactions(stocks, scan);
					totalDaysPurchases = 0;
					totalDaysSales = 0;

					validTransaction = true;
					for (Stock stock : stocks) {
						stock.setTransactionQuantity(Math.floor(stock.getTransactionQuantity() + 0.5));
						if (stock.getTransactionQuantity() > 0) {
							totalDaysPurchases = totalDaysPurchases + stock.getTransactionQuantity() * stock.getStockValue();
						} else {
							totalDaysSales = totalDaysSales - stock.getTransactionQuantity() * stock.getStockValue();
							if (-stock.getTransactionQuantity() > stock.getPortfolioContents()) {
								System.out.println("YOU HAVE OVERSOLD A STOCK; TRY AGAIN.");
								validTransaction = false;
								break;
							}
						}
					}

					//TOTAL VALUE OF TRANSACTIONS:T5
					totalValueOfTransactions = totalDaysPurchases + totalDaysSales;
					// BROKERAGE FEE:B5
					var brokerageFee = Math.floor(0.01 * totalValueOfTransactions * 100 + .5) / 100d;
					// CASH ASSETS=OLD CASH ASSETS-TOTAL PURCHASES
					//-BROKERAGE FEES+TOTAL SALES:C5
					tmpCashAssets = cashAssets - totalDaysPurchases - brokerageFee + totalDaysSales;
					if (tmpCashAssets < 0) {
						System.out.printf("\nYOU HAVE USED $%.2f MORE THAN YOU HAVE.", -tmpCashAssets);
						validTransaction = false;
					} else {
						cashAssets = tmpCashAssets;
					}
				}

				// CALCULATE NEW PORTFOLIO
				for (Stock stock : stocks) {
					stock.setPortfolioContents(stock.getPortfolioContents() + stock.getTransactionQuantity());
				}

				firstRound = false;
			}

		}
	}

	/**
	 * Random int between lowerBound(inclusive) and upperBound(exclusive)
	 */
	private static int randomNumber(int lowerBound, int upperBound) {
		return random.nextInt((upperBound - lowerBound)) + lowerBound;
	}

	private static double newMarketTrendSlope() {
		return randomlyChangeTrendSignAndSlopeAndDuration();
	}

	private static void printPortfolio(boolean firstRound, List<Stock> stocks) {
		//BELL RINGING-DIFFERENT ON MANY COMPUTERS
		if (firstRound) {
			System.out.printf("%n%-30s\t%12s\t%12s", "STOCK", "INITIALS", "PRICE/SHARE");
			for (Stock stock : stocks) {
				System.out.printf("%n%-30s\t%12s\t%12.2f ------ %12.2f", stock.getStockName(), stock.getStockCode(),
						stock.getStockValue(), stock.getChangeStockValue());
			}
			System.out.println("");
		} else {
			System.out.println("\n**********     END OF DAY'S TRADING     **********\n\n");
			System.out.printf("%n%-12s\t%-12s\t%-12s\t%-12s\t%-20s", "STOCK", "PRICE/SHARE",
					"HOLDINGS", "VALUE", "NET PRICE CHANGE");
			for (Stock stock : stocks) {
				System.out.printf("%n%-12s\t%-12.2f\t%-12.0f\t%-12.2f\t%-20.2f",
						stock.getStockCode(), stock.getStockValue(), stock.getPortfolioContents(),
						stock.getStockValue() * stock.getPortfolioContents(), stock.getChangeStockValue());
			}
		}
	}

	private static void readStockTransactions(List<Stock> stocks, Scanner scan) {
		System.out.println("\n\nWHAT IS YOUR TRANSACTION IN");
		for (Stock stock : stocks) {
			System.out.printf("%s? ", stock.getStockCode());

			stock.setTransactionQuantity(readANumber(scan));
		}
	}

	private static int readANumber(Scanner scan) {
		int choice = 0;

		boolean validInput = false;
		while (!validInput) {
			try {
				choice = scan.nextInt();
				validInput = true;
			} catch (InputMismatchException ex) {
				System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
			} finally {
				scan.nextLine();
			}
		}

		return choice;
	}

	private static void adjustAllStockValues(List<Stock> stocks, boolean largeChange1,
			boolean largeChange2,
			double marketTrendSlope,
			Stock stockForLargeChange1, Stock stockForLargeChange2
	) {
		//LOOP THROUGH ALL STOCKS
		for (Stock stock : stocks) {
			double smallChange = random.nextFloat();

			if (smallChange <= 0.25) {
				smallChange = 0.25;
			} else if (smallChange <= 0.5) {
				smallChange = 0.5;
			} else if (smallChange <= 0.75) {
				smallChange = 0.75;
			} else {
				smallChange = 0;
			}

			//BIG CHANGE CONSTANT:W3  (SET TO ZERO INITIALLY)
			var bigChange = 0;
			if (largeChange1) {
				if (stock.getStockCode().equals(stockForLargeChange1.getStockCode())) {
					//ADD 10 PTS. TO THIS STOCK;  RESET E1
					bigChange = 10;
				}
			}

			if (largeChange2) {
				if (stock.getStockCode().equals(stockForLargeChange2.getStockCode())) {
					//SUBTRACT 10 PTS. FROM THIS STOCK;  RESET E2
					bigChange = bigChange - 10;
				}
			}

			stock.setChangeStockValue(Math.floor(marketTrendSlope * stock.stockValue) + smallChange +
					Math.floor(3 - 6 * random.nextFloat() + .5) + bigChange);
			stock.setChangeStockValue(Math.floor(100 * stock.getChangeStockValue() + .5) / 100d);
			stock.stockValue += stock.getChangeStockValue();

			if (stock.stockValue > 0) {
				stock.stockValue = Math.floor(100 * stock.stockValue + 0.5) / 100d;
			} else {
				stock.setChangeStockValue(0);
				stock.stockValue = 0;
			}
		}
	}

	private static double randomlyChangeTrendSignAndSlopeAndDuration() {
		// RANDOMLY CHANGE TREND SIGN AND SLOPE (A), AND DURATION
		var newTrend = Math.floor((random.nextFloat() / 10) * 100 + .5) / 100d;
		var slopeSign = random.nextFloat();
		if (slopeSign > 0.5) {
			newTrend = -newTrend;
		}
		return newTrend;
	}

	private static List<Stock> initStocks() {
		List<Stock> stocks = new ArrayList<>();
		stocks.add(new Stock(100, "INT. BALLISTIC MISSILES", "IBM"));
		stocks.add(new Stock(85, "RED CROSS OF AMERICA", "RCA"));
		stocks.add(new Stock(150, "LICHTENSTEIN, BUMRAP & JOKE", "LBJ"));
		stocks.add(new Stock(140, "AMERICAN BANKRUPT CO.", "ABC"));
		stocks.add(new Stock(110, "CENSURED BOOKS STORE", "CBS"));
		return stocks;
	}

	private static void printGameHelp(Scanner scan) {
		System.out.print("DO YOU WANT THE INSTRUCTIONS (YES-TYPE 1, NO-TYPE 0) ? ");
		int choice = scan.nextInt();
		if (choice >= 1) {
			System.out.println("");
			System.out.println("THIS PROGRAM PLAYS THE STOCK MARKET.  YOU WILL BE GIVEN");
			System.out.println("$10,000 AND MAY BUY OR SELL STOCKS.  THE STOCK PRICES WILL");
			System.out.println("BE GENERATED RANDOMLY AND THEREFORE THIS MODEL DOES NOT");
			System.out.println("REPRESENT EXACTLY WHAT HAPPENS ON THE EXCHANGE.  A TABLE");
			System.out.println("OF AVAILABLE STOCKS, THEIR PRICES, AND THE NUMBER OF SHARES");
			System.out.println("IN YOUR PORTFOLIO WILL BE PRINTED.  FOLLOWING THIS, THE");
			System.out.println("INITIALS OF EACH STOCK WILL BE PRINTED WITH A QUESTION");
			System.out.println("MARK.  HERE YOU INDICATE A TRANSACTION.  TO BUY A STOCK");
			System.out.println("TYPE +NNN, TO SELL A STOCK TYPE -NNN, WHERE NNN IS THE");
			System.out.println("NUMBER OF SHARES.  A BROKERAGE FEE OF 1% WILL BE CHARGED");
			System.out.println("ON ALL TRANSACTIONS.  NOTE THAT IF A STOCK'S VALUE DROPS");
			System.out.println("TO ZERO IT MAY REBOUND TO A POSITIVE VALUE AGAIN.  YOU");
			System.out.println("HAVE $10,000 TO INVEST.  USE INTEGERS FOR ALL YOUR INPUTS.");
			System.out.println("(NOTE:  TO GET A 'FEEL' FOR THE MARKET RUN FOR AT LEAST");
			System.out.println("10 DAYS)");
			System.out.println("-----GOOD LUCK!-----");
		}
		System.out.println("\n\n");
	}

	private static void printIntro() {
		System.out.println("                                STOCK MARKET");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n");
	}

	/**
	 * Stock class also storing the stock information and other related information for simplicity
	 */
	private static class Stock {

		private final String stockName;
		private final String stockCode;
		private double stockValue;
		private double portfolioContents = 0;
		private double transactionQuantity = 0;
		private double changeStockValue = 0;

		public Stock(double stockValue, String stockName, String stockCode) {
			this.stockValue = stockValue;
			this.stockName = stockName;
			this.stockCode = stockCode;
		}

		public String getStockName() {
			return stockName;
		}

		public String getStockCode() {
			return stockCode;
		}

		public double getStockValue() {
			return stockValue;
		}

		public double getPortfolioContents() {
			return portfolioContents;
		}

		public void setPortfolioContents(double portfolioContents) {
			this.portfolioContents = portfolioContents;
		}

		public double getTransactionQuantity() {
			return transactionQuantity;
		}

		public void setTransactionQuantity(double transactionQuantity) {
			this.transactionQuantity = transactionQuantity;
		}

		public double getChangeStockValue() {
			return changeStockValue;
		}

		public void setChangeStockValue(double changeStockValue) {
			this.changeStockValue = changeStockValue;
		}

		@Override
		public String toString() {
			return "Stock{" +
					"stockValue=" + stockValue +
					", stockCode='" + stockCode + '\'' +
					", portfolioContents=" + portfolioContents +
					", transactionQuantity=" + transactionQuantity +
					", changeStockValue=" + changeStockValue +
					'}';
		}
	}

}
