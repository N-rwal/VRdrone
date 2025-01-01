import socket
import random
import struct

# IP and Port settings
UDP_IP = "0.0.0.0"
UDP_PORT = 5005
HEADSET_IP = "192.168.1.52"
HEADSET_PORT = 5006

# Create the socket and bind it to the port
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print(f"Listening for data on {UDP_IP}:{UDP_PORT}...")

# Counter for received messages
message_count = 0

try:
    while True:
        # Receive data from the headset
        data, addr = sock.recvfrom(1024)
        print("Received message:", data.decode('utf-8'))
        message_count += 1

        # Send a response back every 5th message
        if message_count % 5 == 0:
            value1 = random.randint(0, 255)
            value2 = random.randint(0, 255)
            value3 = random.randint(0, 255)
            value4 = random.randint(0, 255)

            message_to_send = struct.pack('BBBB', value1, value2, value3, value4)
            sock.sendto(message_to_send, (HEADSET_IP, HEADSET_PORT))
            print(f"Sent message to headset: {message_to_send}")

except KeyboardInterrupt:
    print("Stopping the script")
finally:
    sock.close()