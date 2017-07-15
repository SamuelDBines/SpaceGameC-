#pragma once
#pragma comment(lib, "ws2_32.lib")
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define filesize 1000
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <string>
#include <iostream>
#include <thread>



class BroadCastLocation {
public:
	BroadCastLocation(int PORT,char* localIPAddressChar);
private:
	bool CreateSocket();
	bool SetBroadcastOption();
	void sendPacket();
private:
	SOCKET sendSocket = 0;
	SOCKADDR_IN sendSockAddr;
	char sendBuffer[filesize];
	std::string message = "";
	char localIPAddressChar[INET_ADDRSTRLEN];
};