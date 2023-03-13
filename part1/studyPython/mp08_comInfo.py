# mp08_comInfo.py
import psutil
import socket
import requests # pip install requests
import re

cpu = (psutil.cpu_freq())

in_addr = socket.gethostbyname(socket.gethostname())
print(in_addr)