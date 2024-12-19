import RPi.GPIO as GPIO
import time
import socket

UDP_IP = "0.0.0.0"
UDP_PORT = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

# Configuration des broches pour les moteurs
motor1_pin = 23  # Broche GPIO pour le moteur 1
motor2_pin = 26  # Broche GPIO pour le moteur 2

# Configuration des broches pour le contrôle PWM
GPIO.setmode(GPIO.BCM)
GPIO.setup(motor1_pin, GPIO.OUT)
GPIO.setup(motor2_pin, GPIO.OUT)

# Configuration des objets PWM
pwm_motor1 = GPIO.PWM(motor1_pin, 10)  # 100 Hz par défaut, ajustez selon vos besoins
pwm_motor2 = GPIO.PWM(motor2_pin, 10)

# Démarrage des PWM avec un rapport cyclique initial de 0 (arrêt)
pwm_motor1.start(0)
pwm_motor2.start(0)

def set_speed(speed_motor1, speed_motor2):
    pwm_motor1.ChangeDutyCycle(speed_motor1)
    pwm_motor2.ChangeDutyCycle(speed_motor2)
try:
    while True:
        data, addr = sock.recvfrom(1024)
        data_str = data.decode('utf-8')

        x_str, y_str = data_str.split(',')  # split by comma
        x = float(x_str)
        y = float(y_str)

        # Convert joystick values to the 0-100 range
        if(x<0):
            x=0
        if(y<0):
            y=0
        x_scaled = int(x * 100)
        y_scaled = int(y * 100)
        print(f"Joystick X: {x_scaled}, Y: {y_scaled}")


        set_speed(x_scaled , y_scaled)


except KeyboardInterrupt:
    # Arrêt propre des moteurs lorsqu'une interruption clavier est détectée
    set_speed(0, 0)
    GPIO.cleanup()