import std.stdio;
import std.random : uniform;

float abs(float num) {
    if(num<0){
        return num*-1;
    }

    return num;
}

void main() {
    
    writeln("\nTIME - SPEED DISTANCE EXERCISE");

    bool keep_playing = true;
    float error_margin = 5.0;

    while(keep_playing){
        int car_speed = uniform!"[]"(40,65);       //Random number between 40 and 65
        int delta_time = uniform!"(]"(4,20);       //Between 5 and 20
        int train_speed = uniform!"[)"(20,40);     //Between 20 and 39; This is the default if not specified: uniform(x,y)

        writeln("\nA CAR TRAVELING AT ", car_speed, " MPH CAN MAKE A CERTAIN TRIP IN ", delta_time,
                " HOURS LESS THAN A TRAIN TRAVELING AT ", train_speed, "MPH." );
        
        float input;
        write("HOW LONG DOES THE TRIP TAKE BY CAR? ");
        readf!"%f\n"(input);

        float car_time = cast(float)delta_time * train_speed / (car_speed - train_speed);
        int percent = cast(int)( abs(car_time-input) * 100 / car_time + .5);

        if(percent > error_margin){
            writeln("SORRY. YOU WERE OFF BY ", percent, " PERCENT.");
        }else{
            writeln("GOOD! ANSWER WITHIN ", percent, " PERCENT.");
        }
        writeln("CORRECT ANSWER IS ", car_time, " HOURS.");

        string answer;
        write("\nANOTHER PROBLEM (YES OR NO)? ");
        readf!"%s\n"(answer);

        if( !(answer == "YES" || answer == "Y" || answer == "yes" || answer == "y") ){
            keep_playing = false;
        }
    }

}