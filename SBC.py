import time
import serial
import socket

# IP and Port settings
UDP_IP = "0.0.0.0"  # listening on all available interfaces
UDP_PORT = 5005      # the used port

# creating socket and binding to the port
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print(f"Listening for data on {UDP_IP}:{UDP_PORT}...")

ser = serial.Serial('/dev/ttyACM0', 115200, timeout=1)  # the usb device

try:
    while True:
        data, addr = sock.recvfrom(1024)
        data_str = data.decode('utf-8')

        x_str, y_str = data_str.split(',')  # split by comma
        x = float(x_str)
        y = float(y_str)

        # Convert joystick values to the 0-100 range
        x_scaled = int(((x + 1) / 2) * 100)
        y_scaled = int(((y + 1) / 2) * 100)

        print(f"Joystick X: {x_scaled}, Y: {y_scaled}")

        # Send the scaled values to the STM32
        # Motor 1 (x-axis) command
        command_x = bytes([0x01, x_scaled])
        ser.write(command_x)

        # Motor 2 (y-axis) command
        command_y = bytes([0x02, y_scaled])
        ser.write(command_y)

except KeyboardInterrupt:
    print("Stopping PWM variation.")

finally:
    ser.close()  # Ensure that the serial port is closed
