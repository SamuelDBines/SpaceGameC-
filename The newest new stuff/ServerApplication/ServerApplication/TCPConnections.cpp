#include "TCPHeader.h"
#include "UDPHeader.h"
#include "loggerHeader.h"
#include "BroadCastHeader.h"
/*-----------------------------------------------------------
TCP Socket connection information
Blocking true
Port 8004
IP address any to local machine
-------------------------------------------------------------*/
#define MALLOC(x) HeapAlloc(GetProcessHeap(), 0, (x))
#pragma comment(lib, "IPHLPAPI.lib")
/*

*/
TCPConnections::TCPConnections(int PORT, bool BroadcastPublic) 
{	
	srand(time(NULL));
	IDListEnabled[0] = true;
	tcpptr = this;

	planetSeed = (rand() % 250) +1;
	if (!initWinSock())	//Winsock creation failed
		return;
	if (!createSocketListen()) //TCP socket
		return;
	setupIPAddress(PORT, BroadcastPublic);
	if (!bindListenSocket())//bind
		return;
	if (!setSocketListen())//Listen
		return;
}

void TCPConnections::setupIPAddress(int PORT, bool BroadcastPublic) 
{
	if (BroadcastPublic) 
	{
		addr.sin_addr.s_addr = htonl(INADDR_ANY);//broadcast publically
		inet_ntop(AF_INET, &(GetLocalHostAddressDetails()), localIPAddressChar, INET_ADDRSTRLEN);
	}
	else 
	{
		addr.sin_addr.s_addr = inet_addr("127.0.0.1");//broadcast locally
		inet_ntop(AF_INET, &(addr.sin_addr), localIPAddressChar, INET_ADDRSTRLEN);
	}
	addr.sin_port = htons(PORT);//port
	addr.sin_family = AF_INET; //IPv4 				
	std::cout << "=> Computer IP Address: " << localIPAddressChar << std::endl;
	
	for (int i = 0;i < INET_ADDRSTRLEN;i++) {
		tcpptr->ThisIpAddress[i] = localIPAddressChar[i];
	}
	BroadCastHandle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)BroadCastHandleThread, (LPVOID)(4), NULL, NULL);

	std::cout << "=> Computer Default Gateway: " <<  getDefaultGateway()<< std::endl;	
	logger servlog("Server", false);//Updatedate serverlog
	servlog.appendServerLog(localIPAddressChar, getDefaultGateway(),PORT);	
}
std::string TCPConnections::getDefaultGateway() 
{
	PIP_ADAPTER_INFO pAdapterInfo;
	DWORD dwRetVal = 0;
	ULONG ulOutBufLen = sizeof(IP_ADAPTER_INFO);
	pAdapterInfo = (IP_ADAPTER_INFO *) MALLOC (sizeof(IP_ADAPTER_INFO));
	if (pAdapterInfo == NULL) 	{
	}
	// Make an initial call to GetAdaptersInfo to get
	// the necessary size into the ulOutBufLen variable
	if (GetAdaptersInfo(pAdapterInfo, &ulOutBufLen) == ERROR_BUFFER_OVERFLOW) {
		pAdapterInfo = (IP_ADAPTER_INFO *)MALLOC(ulOutBufLen);
		if (pAdapterInfo == NULL) {		
		}
	}
	if ((dwRetVal = GetAdaptersInfo(pAdapterInfo, &ulOutBufLen)) == NO_ERROR) {
		std::string dfltgw;
		for (int izjson = 0; izjson < 50; izjson++) {
			std::string checkdf = pAdapterInfo->GatewayList.IpAddress.String;
			if (checkdf != "0.0.0.0") {
				dfltgw = checkdf;
				break;
			}
			pAdapterInfo = pAdapterInfo->Next; // Get next adapter info
		}
		return dfltgw;
	}
}
bool TCPConnections::ListenForNewConnection()
{
	int iRemoteAddrLen = sizeof(SOCKADDR_IN);
	bool bResult = false;
	SOCKET clientConnection;//Socket to hold client connection;
	clientConnection = accept(tcpsocket, (SOCKADDR*)&connectionSockets[connectionCounter].ConnectSockAddr, &iRemoteAddrLen);
	if (INVALID_SOCKET == clientConnection ) {
		return false;
	}
	else {

		updatePlayerLog(true, connectionSockets[connectionCounter].ConnectSockAddr);
		//sendUserID(connectionCounter, clientConnection);	
		connectionSockets[connectionCounter].iConnectSocket = clientConnection;				
		//inet_ntop(AF_INET, &(connectionSockets[connectionCounter].ConnectSockAddr.sin_addr), ipAddress, sizeof(SOCKADDR_IN));		
		if(connectionCounter == 0) {			
			TCPHandle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ClientHandlerThread, (LPVOID)(2), NULL, NULL);
			
			UDPHandle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)udpHandlerThread, (LPVOID)(3), NULL, NULL);			
		}
		char temp[3];
		temp[0] = connectionCounter;
		temp[1] = planetSeed;
		temp[2] = '/0';
		send(connectionSockets[connectionCounter].iConnectSocket, (char *)&temp, 3, 0);	
		connectionSockets[connectionCounter].bInUse = true;
		PlayerCount++;
		IDListEnabled[connectionCounter] = true;
		for (int i = 0;i < MAX_CLIENTS;i++) {
			if (IDListEnabled[i] == false) {
				connectionCounter = i;
				break;
			}
		}
		return true;
	}
}
void TCPConnections::updatePlayerLog(bool connected, SOCKADDR_IN ConnectSockAddr) 
{
		inet_ntop(AF_INET, &(ConnectSockAddr), clientIPAddressChar, INET_ADDRSTRLEN);
		logger playerlog("Player", false);
		playerlog.appendPlayerLog(connected, connectionCounter,"unknown", clientIPAddressChar);
}
void TCPConnections::updateChatLog(int userID, std::string username, std::string chat, std::string ipAddress)
{
	logger playerlog("Chat", false);
	playerlog.appendChatLog(userID, username, chat);
}
void TCPConnections::startListening() {
	listenHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)tcpHandlerThread, (LPVOID)(1), 0, NULL);
}
/* 
TCP Packet proccessing
*/
void TCPConnections::sendUserID(int clientID, SOCKET clientConnection)
{
	byte idPacket = (byte) clientID;
	int iBytesSent = send(clientConnection,(char *)&idPacket, 1, 0);
	if (SOCKET_ERROR == iBytesSent)
	{
		//closeConnection(clientConnection);	// Simplistic - assumes connection has been lost
	}
}
//Threads
void TCPConnections::ClientHandlerThread()
{
	while (true) {
		Sleep(50);
		if (!tcpptr->RecievePacketType())
			break;
	}
}

void TCPConnections::tcpHandlerThread() {
	while (true) {
		Sleep(50);
		tcpptr->ListenForNewConnection();
	}
}
void TCPConnections::udpHandlerThread()
{	
	UDPConnections udp(8002, tcpptr, true);
	udp.clientMessageThread();
}
void TCPConnections::BroadCastHandleThread()
{
	BroadCastLocation broadcast(8000, tcpptr->ThisIpAddress);
}
bool TCPConnections::RecievePacketType()
{
	char buffer[1000] = "";
	for (int i = 0; i < MAX_CLIENTS; i++) {
		int result; 
		result = recv(connectionSockets[i].iConnectSocket, ((char *)&buffer), 1000, 0);
		string temp = buffer;
		if (temp.compare(0, 5, "/quit") == 0){
			IDListEnabled[i] = false;
			PlayerCount--;
			connectionSockets[i] = Connection();
			for (int i = 0;i < MAX_CLIENTS;i++) {
				if (IDListEnabled[i] == false) {
					connectionCounter = i;
					break;
				}
			}
		}
		if(buffer[0] != '\0')
			sendPacketType(i, buffer);
		 memset(&buffer, 0, sizeof(buffer));

	}
	return true;
}
bool TCPConnections::removeClient(int clientID) {
	closesocket(connectionSockets[clientID].iConnectSocket);
	for (int i = clientID; i < connectionCounter; i++) {
		
		connectionSockets[i] = connectionSockets[i + 1];
	}
	connectionCounter--;
	return true;
}
IN_ADDR TCPConnections::GetLocalHostAddressDetails()
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
	if (ppChar != NULL && *ppChar != NULL) {
		pChar = *ppChar;
		addr.S_un.S_un_b.s_b1 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b2 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b3 = (unsigned  char)*pChar++;
		addr.S_un.S_un_b.s_b4 = (unsigned  char)*pChar;
	}
	else {
	}
	return addr;
}
void TCPConnections::sendPacketType(int playerId, char buffer[])
{
	int iBytesSent;
	for (int i = 0; i<MAX_CLIENTS; i++) {
		{
			if (playerId != i) {
				iBytesSent = send(connectionSockets[i].iConnectSocket, buffer, 1000, 0);
				if (SOCKET_ERROR == iBytesSent) {
					//closeConnection(Connections[i]);	// Simplistic - assumes connection has been lost
				}
			}
		}
		
	}
}
bool TCPConnections::closeConnection()
{
	//Terminate threads
	TerminateThread(listenHandle, 0);
	TerminateThread(BroadCastHandle, 0);
	TerminateThread(UDPHandle,0);
	TerminateThread(TCPHandle,0);
	//Close socket. 
	closesocket(tcpsocket);
	if (stopServer == true) {
		std::cout << "=> Socket closed" << std::endl;
	}
	WSACleanup();
	return true;
}

