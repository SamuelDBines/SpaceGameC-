#include "TCPHeader.h"

bool TCPConnections::initWinSock()
{
	WSAData wsadata;
	WORD DllVersion = MAKEWORD(2, 2);
	if (WSAStartup(DllVersion, &wsadata) != 0)
	{
		std::string errorMessage = "=> Failed to setup winsock. Winsock error: " + std::to_string(WSAGetLastError());
		std::cout << errorMessage << std::endl;
		return false;
	}
	std::cout << "=> Winsock succefully connected..." << std::endl;
	return true;
}
bool TCPConnections::createSocketListen()
{
	tcpsocket = socket(AF_INET, SOCK_STREAM, PF_UNSPEC);//Create socket
	if (INVALID_SOCKET == tcpsocket) {
		std::string errorMessage = "=> Failed to create socket. Winsock error: " + std::to_string(WSAGetLastError());
		std::cout << errorMessage << std::endl;
		return false;
	}
	std::cout << "=> TCP Socket was successfully created, socket: " << tcpsocket << std::endl;
	return true;
}
bool TCPConnections::bindListenSocket()
{
	if (SOCKET_ERROR == bind(tcpsocket, (SOCKADDR*)&addr, sizeof(addr))) {
		std::string errorMessage = "=> Failed to bind socket. Winsock error: " + std::to_string(WSAGetLastError());
		std::cout << errorMessage << std::endl;
		return false;
	}
	std::cout << "=> TCP Socket bind was successfully..." << std::endl;
	SetSocketBlockingEnabled(tcpsocket, false);
	return true;
}
bool TCPConnections::setSocketListen()
{
	if (SOCKET_ERROR == listen(tcpsocket, SOMAXCONN)) {
		std::string errorMessage = "=> Failed to listen on socket: " + std::to_string(WSAGetLastError());
		std::cout << errorMessage << std::endl;
		return false;
	}
	std::cout << "=> TCP Socket is ready to accept clients..." << std::endl;
	return true;
}

//int fcntl(serverListen, F_SETFL, O_NONBLOCK);


bool TCPConnections::SetSocketBlockingEnabled(int fd, bool blocking)
{
	if (fd < 0) return false;

#ifdef WIN32
	unsigned long mode = blocking ? 0 : 1;
	return (ioctlsocket(fd, FIONBIO, &mode) == 0) ? true : false;
#else
	int flags = fcntl(fd, F_GETFL, 0);
	if (flags < 0) return false;
	flags = blocking ? (flags&~O_NONBLOCK) : (flags | O_NONBLOCK);
	return (fcntl(fd, F_SETFL, flags) == 0) ? true : false;
#endif
}