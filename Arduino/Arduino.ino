void setup() {
  Serial.begin(9600);
}

void loop() {
  Serial.println(analogRead(A0) / 1024.0 * 5);
  delay(400);
}
