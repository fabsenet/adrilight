#include "FastLED.h"

#define NUM_LEDS (2*73+2*41)
#define LED_DATA_PIN 3
#define NUM_BYTES (NUM_LEDS*3) // 3 colors  

#define BRIGHTNESS 255
#define UPDATES_PER_SECOND 60

#define TIMEOUT 3000

#define MODE_ANIMATION 0
#define MODE_AMBILIGHT 1
#define MODE_BLACK 2
uint8_t mode = MODE_ANIMATION;

byte MESSAGE_PREAMBLE[] = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
uint8_t PREAMBLE_LENGTH = 10;
uint8_t current_preamble_position = 0;

unsigned long last_serial_available = -1L;

int led_counter = 0;
int byte_counter = 0;

CRGB leds[NUM_LEDS];
byte buffer[NUM_BYTES];

// Filler animation attributes
CRGBPalette16 currentPalette = RainbowColors_p;
TBlendType    currentBlending = LINEARBLEND;
uint8_t startIndex = 0;

void setup()
{
    Serial.begin(1000000); // 115200
    FastLED.clear(true);
    FastLED.addLeds<WS2812B, LED_DATA_PIN, GRB>(leds, NUM_LEDS);
    FastLED.setBrightness(BRIGHTNESS);
}

void loop()
{
    switch (mode) {
        case MODE_ANIMATION: 
            fillLEDsFromPaletteColors();
            break;
        
        case MODE_AMBILIGHT:
            processIncomingData();
            break;

            case MODE_BLACK:
            showBlack();
            break;
    }
    
}

void processIncomingData()
{
    if (waitForPreamble(TIMEOUT))
    {
        Serial.readBytes(buffer, NUM_BYTES);

        while (byte_counter < NUM_BYTES)
        {
            byte blue = buffer[byte_counter++];
            byte green = buffer[byte_counter++];
            byte red = buffer[byte_counter++];
            
            leds[led_counter++] = CRGB(red, green, blue);
        }

        FastLED.show();
        
        // flush the serial buffer to avoid flickering
        while(Serial.available()) { Serial.read(); } 
        
        byte_counter = 0;
        led_counter = 0;
    }
    else
    {
        //if we get here, there must have been data before(so the user already knows, it works!)
        //simply go to black!
        mode = MODE_BLACK;  
    }
}

bool waitForPreamble(int timeout)
{
    last_serial_available = millis();
    current_preamble_position = 0;
    while (current_preamble_position < PREAMBLE_LENGTH)
    {
        if (Serial.available() > 0)
        {
            last_serial_available = millis();

            if (Serial.read() == MESSAGE_PREAMBLE[current_preamble_position])
            {
                current_preamble_position++;
            }
            else
            {
                current_preamble_position = 0;
            }
        }

        if (millis() - last_serial_available > timeout)
        {
            return false;
        }
    }
    return true;
}

void fillLEDsFromPaletteColors()
{
    startIndex++;

    uint8_t colorIndex = startIndex;
    for( int i = 0; i < NUM_LEDS; i++) {
            leds[i] = ColorFromPalette(currentPalette, colorIndex, BRIGHTNESS, currentBlending);
            colorIndex += 3;
        }

    FastLED.delay(1000 / UPDATES_PER_SECOND);

    if (Serial.available() > 0)
    {
        mode = MODE_AMBILIGHT;
    }
}

void showBlack()
{     
    for( int i = 0; i < NUM_LEDS; i++) 
    {
        leds[i] = CRGB(0,0,0);
    }
    FastLED.delay(1000 / UPDATES_PER_SECOND);

    if (Serial.available() > 0)
    {
        mode = MODE_AMBILIGHT;
    }
}

