#include "BroadCastHeader.h"
#include "loggerHeader.h"
/*
BroadCastLocation(int) - Sets up the server broadcast for clients to connect
Createsocket() - creates the UDP send socket
setBroadcastOption() - Sets the broadcast option
sendPacket() - sends a string on broadcast
*/
BroadCastLocation::BroadCastLocation(int PORT, char* ThisIPAddressChar)
{

	// Create the Send Socket
	if (!CreateSocket())
	{	
	}
	//Set the broadcast option to true
	message = "QZX ";
	message.assign(localIPAddressChar, 22);
	message = "QZX " + (string)localIPAddressChar + " " + to_string(PORT);
	SetBroadcastOption();
	sendSockAddr.sin_addr.s_addr = INADDR_BROADCAST;//Broadcast accross all addresses
	sendSockAddr.sin_family = AF_INET;//IPV4 family
	sendSockAddr.sin_port = htons(PORT);//Allows big endian to little endian (mainly used for different OS systems)
	inet_ntop(AF_INET, &(sendSockAddr.sin_addr), localIPAddressChar, INET_ADDRSTRLEN);
	logger broadcastlog("Server", false);
	broadcastlog.appendServerLog(localIPAddressChar,"null", PORT);
	message = "QZX " + (string)ThisIPAddressChar + " " + to_string(PORT+1)+" Hello, welcome to the server";

	while (true) {
		Sleep(500);//Pulse a message every half second
		sendPacket();
	}
}

bool BroadCastLocation::CreateSocket()
{
	sendSocket = socket(AF_INET, SOCK_DGRAM, PF_UNSPEC);
	if (INVALID_SOCKET == sendSocket)
	{
		return false;
	}
	return true;
}
bool BroadCastLocation::SetBroadcastOption()
{
	char cOpt[2];
	cOpt[0] = 1;//true = 1 | false = 0
	cOpt[1] = '\0';//Null pointer
	int iError = setsockopt(sendSocket, SOL_SOCKET, SO_BROADCAST, cOpt, sizeof(cOpt)); //set the socket
	if (SOCKET_ERROR == iError)
	{
		return false;
	}
	return true;
}
void BroadCastLocation::sendPacket()
{
	
	strncpy_s(sendBuffer, message.c_str(), filesize);//Copy the message to the send buffer
	int bytesSent = sendto(sendSocket,//Socket
		(char*)sendBuffer,//data
		filesize,//datasize
		0, (const struct sockaddr*)&sendSockAddr,//Address
		sizeof(sendSockAddr));//address size (depends on IPV4|IPV6)
	if (INVALID_SOCKET == bytesSent)//FAILED
	{
		//do something
	}
}
