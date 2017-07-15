#include "TCPHeader.h"
#include "UDPHeader.h"


UDPConnections::UDPConnections(int PORT,  TCPConnections* pointerInformation,bool BroadcastPublic)
{
		
		// Create the Send Socket
		if (!CreateSocket())
		{	// Exit the application if the socket could not be created
			//
		}
		// Set up the socket address structure with the local address
		if (BroadcastPublic)
			recieveSockAddr.sin_addr.s_addr = htonl(INADDR_ANY);//broadcast publically
		else
			recieveSockAddr.sin_addr.s_addr = inet_addr("127.0.0.1");//broadcast locally
		recieveSockAddr.sin_family = AF_INET;
		recieveSockAddr.sin_port = htons(8002); 
		SetBroadcastOption();
		sendSockAddr.sin_addr.s_addr = INADDR_BROADCAST;
		sendSockAddr.sin_family = AF_INET;
		sendSockAddr.sin_port = htons(8003);		
		// Bind to the local address
		if (!BindToLocalAddress())
		{		// Exit the application if the bind was not successful
			return;
		}
		udppointer = this;
		tcpptr = pointerInformation;
		
}
//Setup UDP send and recive Sockets
bool UDPConnections::CreateSocket()
{

	sendSocket = socket(AF_INET, SOCK_DGRAM, PF_UNSPEC);
	if (INVALID_SOCKET == sendSocket)
	{
		return false;
	}
	recieveSocket = socket(AF_INET, SOCK_DGRAM, PF_UNSPEC);
	if (INVALID_SOCKET == recieveSocket)
	{
		return false;
	}
	return true;
}
bool UDPConnections::BindToLocalAddress()
{
	int iError = bind(recieveSocket, (SOCKADDR*)&recieveSockAddr, sizeof(recieveSockAddr));
	if (SOCKET_ERROR == iError)
	{
		return false;
	}
	return true;
}
// CSimpleReceiveDialog message handler
void UDPConnections::recievePacket()
{
	int len;
	char temp[filesize * 10];	
	int iBytesRecd = recvfrom(recieveSocket, (char*)recieveBuffer, filesize, 0, NULL, NULL);
	//recvfrom(recieveSocket, (char*)temp, filesize*10, 0, NULL, NULL);
	if (SOCKET_ERROR == iBytesRecd)
	{
		//error
	}
	else
	{
		if (checkPattern(recieveBuffer) == true) {	
		
			int clientid = recieveBuffer[5];

			if (clientid < 0) {

			} else {				
				updatePacket(0);
				recieveBuffer[iBytesRecd] = 0; // ensure null termination
				sendPacket(0);
			}
		}
	}
}
bool UDPConnections::knownIps() {
	for (int i = 0; i < conncounter; i++) {
		if (sendSockAddr.sin_addr.s_addr == ipser[i].ips.sin_addr.s_addr) {
			continue;
		}
		else {

		}
	}
	return false;
}
bool UDPConnections::checkPattern(char* buffer) {
	if (buffer[0] != protocol)
		return false;
	if (buffer[1] != checkOne)
		return false;
	if (buffer[2] != checkTwo)
		return false;
	if (buffer[3] != checkThree)
		return false;
	if (buffer[4] != server)
		return false;
	return true;
}
UDPConnections::~UDPConnections()	
{	
	closesocket(recieveSocket);
	//closesocket(UDPConn.m_SendSOCKET);
}




bool UDPConnections::SetBroadcastOption()
{
	char cOpt[2];
	cOpt[0] = 1;
	cOpt[1] = '\0';
	int iError = setsockopt(sendSocket, SOL_SOCKET,SO_BROADCAST,cOpt,sizeof(cOpt));
	if (SOCKET_ERROR == iError)
	{
		return false;
	}
	return true;
}
IN_ADDR UDPConnections::GetLocalHostAddressDetails()
{
	// This is the most complicated aspect of addressing.
	// You probably don't need to understand this method,
	// just use it as is.
	char szStr[80];
	DWORD lLen = sizeof(szStr);
	GetComputerNameA(szStr, &lLen);
	hostent* pHost;
	pHost = gethostbyname(szStr);

	IN_ADDR addr;
	char*  pChar;
	char** ppChar = pHost->h_addr_list;

	if (ppChar != NULL && *ppChar != NULL)
	{
		pChar = *ppChar;
		addr.S_un.S_un_b.s_b1 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b2 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b3 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b4 = (unsigned  char)*pChar;
	}
	else
	{
		
	}
	return addr;
}




void UDPConnections::updatePacket(int index)
{
	for (int i = 0; i < filesize; i++)
	{
		sendBuffer[i] = recieveBuffer[i];
		sendBuffer[22] = tcpptr->PlayerCount;
	}
}


// CSimpleSendDialog message handlers
void UDPConnections::sendPacket(int index)
{
	sendBuffer[4] = 'C';
	// Send the datagram
	int iBytesSent;
	iBytesSent = sendto(sendSocket, (char*)sendBuffer, filesize+1, 0,(const struct sockaddr*)&sendSockAddr, sizeof(sendSockAddr));
	if (INVALID_SOCKET == iBytesSent)
	{

	}
}

void UDPConnections::clientMessageThread()
{
	while (true) {
		recievePacket();
	}
}