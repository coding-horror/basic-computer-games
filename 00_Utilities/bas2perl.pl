#!/usr/bin/perl
use strict;


my $Mode= lc($ARGV[0]);  #trace #convert
my $File= $ARGV[1];
my $LN= "Line";

my %Vars;
my @Data;
my %Code;
open(FH, $File);
while (my $Line = <FH>) {
	chomp $Line;
	my $Space= index($Line, " ");
	my $Key= substr($Line, 0, $Space);
	my $Code= substr($Line, $Space+1);
	$Code{$Key}=$Code;
	}
close(FH);


foreach my $Lin (sort  {$a<=>$b} keys %Code) {
	if ($Mode eq "trace") { print "==> $Lin $Code{$Lin}\n"; }
	my $Ret= &PROCLINE($Code{$Lin});
	if ($Mode eq "trace") { print "   $Ret\n"; }
	$Code{$Lin}= $Ret;
	}


if ($Mode eq "convert") {
	$Code{'0.1'}= "#!/usr/bin/perl";
	$Code{'0.2'}= "#use strict;";
	$Code{'0.3'}= "# Automatic converted by bas2perl.pl";
	$Code{'0.4'}= "";
	foreach my $Lin (sort  {$a<=>$b} keys %Code) {
		print "$Code{$Lin}\n";
		}
	}

	if (@Data) { &DATAIL(); }
	print "\n\n\n";


exit;



#print @Lines;

sub PROCLINE {
	my ($Line)= @_;
	#my @Sente= split(/:/, $Line);
	#my @Sente= split(/\:(?=([^"]*"[^"]*")*[^"]*$)/g, $Line);
	my @Sente= split(/:(?=(?:[^"]*"[^"]*")*[^"]*$)/g, $Line);

	my $Perl;
	foreach my $Sen (@Sente) {
		#if ($Sen eq "") { next; } # Somehow the regex gives empty items between.
		my $Flag=0;
		$Sen= &TRIM($Sen);
		if ($Sen>0) { $Sen= "GOTO $Sen"; } #The birth of spaguetti code!
		if ($Sen=~ /^DATA\b/) { $Sen= &DATA($Sen); $Flag=1; }
		if ($Sen=~ /^DIM\b/) { $Sen= &DIM($Sen); $Flag=1; }
		if ($Sen=~ /^END\b/) { $Sen= &ENDD($Sen); $Flag=1; }
		if ($Sen=~ /^FOR\b/) { $Sen= &FOR($Sen); $Flag=1; }
		if ($Sen=~ /^GOTO\b/) { $Sen= &GOTO($Sen); $Flag=1; }
		if ($Sen=~ /^GOSUB\b/) { $Sen= &GOSUB($Sen); $Flag=1; }
		if ($Sen=~ /^IF\b/) { $Sen= &IF($Sen); $Flag=1; }
		if ($Sen=~ /^INPUT\b/) { $Sen= &INPUT($Sen); $Flag=1; }
		if ($Sen=~ /^NEXT\b/) { $Sen= &NEXT($Sen); $Flag=1; }
		if ($Sen=~ /^ON\b/ && $Sen=~ / GOTO /) { $Sen= &ONGOTO($Sen); $Flag=1; }
		if ($Sen=~ /^PRINT\b/) { $Sen= &PRINT($Sen); $Flag=1; }
		if ($Sen=~ /^READ\b/) { $Sen= &READ($Sen); $Flag=1; }
		if ($Sen=~ /^REM\b/) { $Sen= &REM($Sen); $Flag=1; }
		if ($Sen=~ /^RETURN\b/) { $Sen= &RETURN($Sen); $Flag=1; }
		if ($Sen=~ /^STOP\b/) { $Sen= &ENDD($Sen); $Flag=1; }
		if ($Flag==0) { $Sen= &FORMULA($Sen); }
		$Sen.=";";
		$Sen=~ s/\{;$/\{/g;
		$Sen=~ s/\};$/\}/g;
		$Perl.= "$Sen ";
		}
	$Perl= &TRIM($Perl);
	return $Perl;
	}



####################
# BASIC STATEMENTS #
####################

sub DATA {
	my ($Str)= @_;
	$Str=~ s/DATA //;
	push @Data, $Str;
	return "# TO DATA SEGMENT";
	}

sub DIM {
	my ($Str)= @_;
	$Str=~ s/DIM //;
	my @Parts= split(/\,(?![^\(]*\))/, $Str);
	my $Out;
	foreach my $Par (@Parts) {
		$Par=~ s/\$//;
		$Par=~ s/\(.*\)//;
		$Out.= "my \@$Par; ";
		$Vars{$Par}= "array";
		}
	chop $Out;
	chop $Out;
	return $Out;
	}


sub ENDD {
	return "exit";
	}


sub FOR {
	my ($Str)= @_;
	$Str=~ s/= /=/g;
	my @Parts= split(" ", $Str);
	$Parts[1]= &FORMULA($Parts[1]);
	my $Var=substr($Parts[1],0,index($Parts[1],"="));
	$Parts[3]= "$Var<=".&FORMULA($Parts[3]);
	if ($Parts[5]<0) { $Parts[3]=~ s/</>/; }
 	$Parts[5]= $Parts[5] eq "" ? "$Var++" : "$Var+=".&FORMULA($Parts[5]);
	$Str= "for ($Parts[1]; $Parts[3]; $Parts[5]) {"; 
	return $Str;
	}


sub GOTO {
	my ($Str)= @_;
	my @Parts= split(" ", $Str);
	my $Label= "$LN$Parts[1]";
	$Str= lc($Parts[0])." $Label";
	$Code{($Parts[1]-.2)}="";
	$Code{($Parts[1]-.1)}="$Label:";
	return $Str;
	}


sub GOSUB {
	my ($Str)= @_;
	my @Parts= split(" ", $Str);
	my $Label= "$LN$Parts[1]";
	$Str= "\&$Label()";
	$Code{($Parts[1]-.2)}="";
	$Code{($Parts[1]-.1)}="sub $Label {";
	return $Str;
	}


sub IF {
	my ($Str)= @_;
	$Str=~ s/^IF //g;
	my @Parts= split(" THEN ", $Str);
	$Parts[0]= &FORMULA($Parts[0], 1);
	$Parts[1]= &PROCLINE($Parts[1]);
	my $Str= "if ($Parts[0]) { $Parts[1] }";
	return $Str;
	}


sub INPUT {
	my ($Str)= @_;
	$Str=~ s/INPUT //;
	$Str=~ s/(".*")//g;
	my $Ques;
	if ($1) {
		$Ques= $1;
		$Ques=~ s/"//g;
		}

	$Str=~ s/\$//g;
	$Str=~ s/;//g;
	$Str=~ s/\(/\[/g;
	$Str=~ s/\)/\]/g;
	my $Inp;
	if ($Str=~ /,/) {
		$Str= "\$$Str";
		$Str=~ s/,/,\$/g;
		$Inp= "; ($Str)= split(/,/, \$Inp)";
		$Str= "Inp";
		}
	$Str= "print \"$Ques? \"; chomp(\$$Str = uc(<STDIN>))";
	return $Str.$Inp;
	}


sub NEXT {
	return "}";
	}


sub ONGOTO {
	# Base 1, if not match it will be skipped.
	my ($Str)= @_;
	my @Parts= split(" ", $Str);
	my $Var= $Parts[1];
	my @Lines= split(",", $Parts[3]);
	my $Count=0;
	my $Text;
	foreach my $Lin (@Lines) {
		$Count++;
		my $This= "\telsif (\$$Var==$Count) ";
		if ($Count==1) { $This= "if (\$$Var==1) "; }

		my $Goto= &GOTO("GOTO $Lin");
		$This.="{ $Goto; }\n";
		$Text.= $This;
		}
	return $Text;
	}


sub PRINT {
	my ($Str)= @_;
	if ($Str eq "PRINT") { return 'print "\n"' };
	$Str=~ s/^PRINT //;

	my $Enter= 1;
	if ($Str=~ /;$/) { $Enter= 0; }

	my $Out;
	#my @Parts= split(/;/, $Str);
	my @Parts= split(/;(?=(?;[^"]*"[^"]*")*[^"]*$)/g, $Str);

	foreach my $Par (@Parts) {
		if ($Par=~ / TAB\((.*?)\);/) {
			my $Num= &FORMULA($1);
			$Par=~ s/ TAB\(.*\);/' 'x$Num . /;
			}
		if ($Par=~ /^[A-Z]/) {
			$Par= &FIXVAR($Par);
			}
		$Out.= $Par.". ";
		}
	chop $Out;
	chop $Out;
	#$Str=~ s/"$/\\n"/;
	#$Str=~ s/;$//;
	#$Str=~ s/;/\./g;
	#$Str=~ s/"$/\\n"/g;
	#$Str=~ s/\.$//g;
	if ($Enter) { $Out.= qq|. "\\n"|; }
	return "print ".$Out;
	}


sub READ {
	my ($Str)= @_;
	$Str=~ s/READ //;
	$Str.="= <DATA>";
	return $Str;
	}


sub REM {
	my ($Str)= @_;
	return "#".$Str;
	}


sub RETURN {
	return "return; }";
	}


###########
# HELPERS #
###########

sub TRIM {
	my ($Str)= @_;
	#$Str=~ s/\s+/ /g;
	$Str=~ s/^\s+//;
	$Str=~ s/\s+$//;
	return $Str;
	}


sub DATAIL {
	print "\n\n\n";
	print "__DATA__\n";
	foreach my $Dat (@Data) {
		$Dat=~ s/"//g;
		$Dat=~ s/,/\n/g;
		print "$Dat\n";
		}
	}


sub FORMULA {
	my ($Str, $Cond)= @_;
	$Str=~ s/\$//g;
	$Str=~ s/ABS\(/abs\(/;
	$Str=~ s/COS\(/cos\(/;
	$Str=~ s/LEN\(/length\(/;
	$Str=~ s/INT\(/int\(/;
	$Str=~ s/MID\$?\(/substr\(/;
	$Str=~ s/RND\(/rand\(/;
	$Str=~ s/SIN\(/sin\(/;
	$Str=~ s/(\b[A-Z][0-9]?\b)/\$$&/g;
	if ($Cond==1) {
		$Str=~ s/<>/ ne /g;
		$Str=~ s/=/ eq /g;
		}
	return $Str;
	}


sub FIXVAR {
	my ($Str)= @_;
	$Str=~ s/\$//g;
	$Str=~ s/(\w+)/\$$1/g;
	$Str=~ s/\(/\[/g;
	$Str=~ s/\)/\]/g;
	$Str=~ s/,/\]\[/g;
	return $Str;
	}



