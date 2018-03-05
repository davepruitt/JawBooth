#include <EEPROM.h>

int booth_name_eeprom_address = 0;
int input_state = 0;
int stream_enable = 0;

int np_history_size = 100;
int np_history_idx = 0;
int np_history[100];
bool np_state = false;

int NOSEPOKE_CUTOFF_VALUE = 500;

int TRIGGER_FEEDER_INCOMING_MESSAGE = 70;
int TRIGGER_STIMULATOR_INCOMING_MESSAGE = 84;
int SAVE_NEW_BOOTH_NAME_INCOMING_MESSAGE = 83;
int READ_CURRENT_BOOT_NAME_INCOMING_MESSAGE = 76;
int STREAM_ENABLE_INCOMING_MESSAGE = 69;
int STREAM_DISABLE_INCOMING_MESSAGE = 68;

int NOSEPOKE_IN_OUTGOING_MESSAGE = 49;
int NOSEPOKE_OUT_OUTGOING_MESSAGE = 48;


int NOSEPOKE_INPUT_PIN = A0;
int STIMULATOR_PIN = A1;
int FEEDER_PIN = A2;


void setup()
{
    //Initialize the nosepoke history
    memset(np_history, 0, sizeof(int) * np_history_size);
    
    //Set the pins to the proper input mode
    pinMode(NOSEPOKE_INPUT_PIN, INPUT);
    pinMode(FEEDER_PIN, OUTPUT);
    pinMode(STIMULATOR_PIN, OUTPUT);

    //Keep the feeder pin high (we will send it low when we want to feed)
    digitalWrite(FEEDER_PIN, HIGH);
    
    //Initialize serial communication
    Serial.begin(115200);
    Serial.println("READY");
}

void loop()
{
    //Check to see if we have received a command from the computer
    if (Serial.available() > 0)
    {
        //Read the incoming command
        int incoming_byte = Serial.read();

        if (input_state == 0)
        {
            if (incoming_byte == TRIGGER_FEEDER_INCOMING_MESSAGE)
            { 
                //If the command is to trigger a feed...
                digitalWrite(FEEDER_PIN, LOW);
                delay(5);
                digitalWrite(FEEDER_PIN, HIGH);
            }
            else if (incoming_byte == TRIGGER_STIMULATOR_INCOMING_MESSAGE)
            {
                //If the command is to trigger the stimulator...
                digitalWrite(STIMULATOR_PIN, HIGH);
                delay(50);
                digitalWrite(STIMULATOR_PIN, LOW);
            }
            else if (incoming_byte == SAVE_NEW_BOOTH_NAME_INCOMING_MESSAGE)
            {
                //Set the input state to 1. On the subsequent iteration of the main loop, 
                //we will read the next byte on the Serial line, which will be the booth name, 
                //and it will be handled accordingly.
                input_state = 1; 
            }
            else if (incoming_byte == READ_CURRENT_BOOT_NAME_INCOMING_MESSAGE)
            {
                //Grab the booth number from EEPROM
                char current_booth_name = (char) EEPROM.read(booth_name_eeprom_address);

                //Write the booth number to the serial line
                Serial.println(current_booth_name);
            }
            else if (incoming_byte == STREAM_ENABLE_INCOMING_MESSAGE)
            {
                stream_enable = 1;
            }
            else if (incoming_byte == STREAM_DISABLE_INCOMING_MESSAGE)
            {
                stream_enable = 0;
            }
        }
        else if (input_state == 1)
        {
            //If the input state is 1, save the current byte as the new booth name
            EEPROM.write(booth_name_eeprom_address, incoming_byte);

            //Return the input state to be 0
            input_state = 0;
        }
    }

    //Read the latest values from the nosepokes
    int np_value = analogRead(NOSEPOKE_INPUT_PIN);

    //Stream the latest nosepoke value if streaming is turned on
    if (stream_enable == 1)
    {
        Serial.println(np_value);
    }

    /*
    //Store the values in the history for each nosepoke
    np_history[np_history_idx] = np_value;
    
    //Increment the storage index
    np_history_idx++;
    if (np_history_idx >= np_history_size)
    {
        np_history_idx = 0;
    }

    //Compute the average nosepoke value from the last 100 samples
    float np_sum = 0;
    for (int i = 0; i < np_history_size; i++)
    {
        np_sum += np_history[i];
    }

    float np_avg = np_sum / float(np_history_size);

    //Determine what message to send to the computer
    if (np_avg >= NOSEPOKE_CUTOFF_VALUE)
    {
        if (!np_state)
        {
            np_state = true;
            Serial.println(NOSEPOKE_IN_OUTGOING_MESSAGE);
        }
    }
    else
    {
        if (np_state)
        {
            np_state = false;
            Serial.println(NOSEPOKE_OUT_OUTGOING_MESSAGE);
        }
    }
    */
}











