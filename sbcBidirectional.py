import socket
import random
import struct

# IP and port settings
UDP_IP = "0.0.0.0"
UDP_PORT = 5005
HEADSET_IP = "192.168.1.52"
HEADSET_PORT = 5006

# create the socket and bind it to the port
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print(f"listening for data on {UDP_IP}:{UDP_PORT}...")

message_count = 0

try:
    while True:
        # here it receives the data (user's pilot commands)
        data, addr = sock.recvfrom(1024)
        print("received message:", data.decode('utf-8'))
        message_count += 1

        # sends a message every 5th received message, may need to be increased
        if message_count % 5 == 0:
            value1 = random.randint(0, 255)
            value2 = random.randint(0, 255)
            value3 = random.randint(0, 255)
            value4 = random.randint(0, 255)

            #packing the message to send
            message_to_send = struct.pack('BBBB', value1, value2, value3, value4)
            sock.sendto(message_to_send, (HEADSET_IP, HEADSET_PORT))
            print(f"sent message: {message_to_send}")

except KeyboardInterrupt:
    print("quit the script")
finally:
    sock.close()