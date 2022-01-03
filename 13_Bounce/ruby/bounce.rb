## Global constants

# Gravity accelaration (F/S^2) ~= 32
G = 32

# Used to indent the plotting of ball positions
# so that the height digits don't affect
# where we start plotting ball positions
BALL_PLOT_INDENT = "\t"

# The deviation between current plotted height and the actual
# height of the ball that we will accept to plot the ball in
# that plotted height
BALL_PLOT_DEVIATION = 0.25

# The step we will take as we move down vertically while
# plotting ball positions
BALL_PLOT_HEIGHT_STEP = 0.5


## Helper functions

# Calculates the bounce speed (up) of the ball for a given
# bounce number and coefficient
def calc_velocity_for_bounce(v0, bounce, coefficient)
    v = v0 * coefficient**bounce
end

# Check https://physics.stackexchange.com/a/333436 for nice explanation
def calc_bounce_total_time(v0, bounce, coefficient)
    v = calc_velocity_for_bounce(v0, bounce, coefficient)
    t = 2 * v / G
end

# Check https://physics.stackexchange.com/a/333436 for nice explanation
def calc_ball_height(v0, bounce, coefficient, t)
    v = calc_velocity_for_bounce(v0, bounce, coefficient)
    h = v * t - 0.5 * G * t**2
end

def heighest_position_in_next_bounce(time_in_bounce, v0, i, c)
    time_in_next_bounce = time_in_bounce[i+1]
    return -1 if time_in_next_bounce.nil?
    return calc_ball_height(v0, i, c, time_in_next_bounce / 2) unless time_in_next_bounce.nil?
end

def intro
    puts <<~INSTRUCTIONS
                    BOUNCE
    CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY


    THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY
    OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF
    ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION
    COEFFICIENCY (LESS THAN 1).

    YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN
    'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).
    INSTRUCTIONS
end


## Plottin functions

def plot_header
    puts
    puts "FEET"
end

def plot_bouncing_ball(strobbing_time, v0, c)
    ## Initializing helper values

    # How many bounces we want to plot
    # original BASIC version is 70 / (V / (16 * S2))
    # 70 is assumed to be an arbitrary number higher than 2G and 16 is 1/2G
    bounces_to_plot = (G**2 / (v0 / strobbing_time)).to_i

    # Holds the total time the ball spends in the air in every bounce
    time_in_bounce = bounces_to_plot.times.map { |i| calc_bounce_total_time v0, i, c }

    plot_width = 0

    # Calculate the highest position for the ball after the very first bounce
    plotted_height = (calc_ball_height(v0, 0, c, v0/G) + 0.5).to_i

    ## Plotting bouncing ball
    while plotted_height >= 0 do
        # We will print only whole-number heights
        print plotted_height.to_i if plotted_height.to_i === plotted_height

        print BALL_PLOT_INDENT

        bounces_to_plot.times { |i|
            (0..time_in_bounce[i]).step(strobbing_time) { |t|
                ball_pos = calc_ball_height v0, i, c, t

                # If the ball is within the acceptable deviation
                # from the current height, we will plot it
                if (plotted_height - ball_pos).abs <= BALL_PLOT_DEVIATION then
                    print "0"
                else
                    print " "
                end

                # Increment the plot width when we are plotting height = 0
                # which will definitely be the longest since it never gets
                # skipped by line 98
                plot_width += 1 if plotted_height == 0
            }
            
            if heighest_position_in_next_bounce(time_in_bounce, v0, i, c) < plotted_height then
                # If we got no more ball positions at or above current height, we can skip
                # the rest of the bounces and move down to the next height to plot
                puts
                break
            end
        }

        plotted_height -= BALL_PLOT_HEIGHT_STEP
    end

    # Return plot_width to be used by the plot_footer
    plot_width
end

def plot_footer (plot_width, strobbing_time)
    # Dotted separator line
    puts
    print BALL_PLOT_INDENT
    (plot_width).times { |_| print "." }
    puts

    # Time values line
    print BALL_PLOT_INDENT
    points_in_sec = (1 / strobbing_time).to_i
    plot_width.times { |i|
        if i % points_in_sec == 0 then
            print (i / points_in_sec).to_i
        else
            print " "
        end
    }
    puts

    # Time unit line
    print BALL_PLOT_INDENT
    (plot_width / 2 - 4).to_i.times { |_| print " " }
    puts "SECONDS"
    puts
end

def game_loop
    # Read strobing, velocity and coefficient parameters from user input
    puts "TIME INCREMENT (SEC)"
    strobbing_time = gets.to_f

    puts "VELOCITY (FPS)"
    v0 = gets.to_f

    puts "COEFFICIENT"
    c = gets.to_f

    # Plotting
    plot_header

    plot_width = plot_bouncing_ball strobbing_time, v0, c

    plot_footer plot_width, strobbing_time
end


## Game entry point

begin
    intro
    while true
        game_loop
    end
rescue SystemExit, Interrupt
    exit
rescue => exception
   p exception
end
