class Calendar
  def initialize
    $stdout.sync = true    
  end

  def parse_input
    days_mapping = {
      "sunday": 0,
      "monday": -1,
      "tuesday": -2,
      "wednesday": -3,
      "thursday": -4,
      "friday": -5,
      "saturday": -6,
    }

    day = 0
    leap_day = false
    correct_day_input = false

    while !correct_day_input 
      print "INSERT THE STARTING DAY OF THE WEEK OF THE YEAR: "
      weekday = gets.chomp!

      days_mapping.each do |k, v|
        if k.to_s == weekday.downcase.to_s
          day = v
          correct_day_input = true
          break
        end
      end
    end

    while true
      print "IS IT A LEAP YEAR?: "
      leap = gets.chomp!
      if "y" == leap.downcase.to_s
        leap_day = true
        break
      end

      if "n" == leap.downcase.to_s
        leap_day = false
        break
      end
    end

    return day, leap_day
  end

  def calendar(weekday, leap_year)
    months_days = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    days = "S        M        T        W        T        F        S\n"
    sep = "*" * 59
    years_day = 365
    d = weekday

    if leap_year
      months_days[2] = 29
      years_day = 366
    end

    months_names = [
      " JANUARY ",
      " FEBRUARY",
      "  MARCH  ",
      "  APRIL  ",
      "   MAY   ",
      "   JUNE  ",
      "   JULY  ",
      "  AUGUST ",
      "SEPTEMBER",
      " OCTOBER ",
      " NOVEMBER",
      " DECEMBER",
    ]

    days_count = 0

    for n in (1..12) do
      days_count += months_days[n - 1]
      print "** #{days_count} ****************** #{months_names[n - 1]} ****************** #{years_day - days_count} **\n"
      print days
      print sep
      for nnn in (1..6) do
        print "\n"
        for g in (1..7) do
          d += 1
          d2 = d - days_count

          break if d2 > months_days[n]

          if d2 <= 0
            print "         "
          elsif d2 < 10
            print " #{d2}       "
          else
            print "#{d2}       "
          end
        end

        print "\n\n"

        break if d2 >= months_days[n]
      end
      d -= g if d2 > months_days[n]
      print "\n"
    end
    print("\n")
  end
end


calendar = Calendar.new
input = calendar.parse_input

calendar.calendar(input[0], input[1])

