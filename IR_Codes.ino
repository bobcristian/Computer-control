#include <boarddefs.h>
#include <IRremote.h>
#include <IRremoteInt.h>
#include <ir_Lego_PF_BitStreamEncoder.h>

const int recievePin = 10;
IRrecv irrecv(recievePin);
decode_results results;

void setup() 
{
  Serial.begin(9600);
  irrecv.enableIRIn();
  pinMode(13, OUTPUT);
  digitalWrite(13,LOW);
}

void loop() 
{
  if(irrecv.decode(&results))
  {
   Serial.println(results.value,HEX);
   irrecv.resume(); 
  }
  delay(10);
}
