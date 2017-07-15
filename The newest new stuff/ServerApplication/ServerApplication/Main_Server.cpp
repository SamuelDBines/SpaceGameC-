//Server

#include "TCPHeader.h"
#include "UDPHeader.h"
#include "loggerHeader.h"
#include <iostream>
#include <thread>
#include <io.h>
#include <fcntl.h>

int main()
{

	int PORT;
	char localComputer;
	bool BroadCastPublic,
		serverstart;
	std::cout << "=> Welcome to the server "  << std::endl;
	std::cout << "=> Enter port number: ";
	std::cin >> PORT;
	std::cout << "=> Local computer (Y/N): ";
	std::cin >> localComputer;
	switch (localComputer) {
	case 'Y':
	case 'y':
		BroadCastPublic = false;
		break;
	default:
		BroadCastPublic = true;
		break;

	}
	TCPConnections tcp(PORT, BroadCastPublic);
	serverstart = true;
	tcp.startListening();
	//serverRun.connectSQL();
	while(true) {
		std::cout << "=> Enter command: ";
		std::string command;
		std::getline(cin, command);
		if (_stricmp(command.c_str(), "Start") ==0) {//Start server
			if(serverstart)
				std::cout << "Server is already running" << std::endl;
			else {
				std::cout << "=> Enter port number: ";
				std::cin >> PORT;
				std::cout << "=> Local computer (Y/N): ";
				std::cin >> localComputer;
				switch (localComputer) {
				case 'Y':
				case 'y':
					BroadCastPublic = false;
					break;
				default:
					BroadCastPublic = true;
					break;

				}
				TCPConnections tcp(PORT, BroadCastPublic);
				serverstart = true;
			}
		} else if (_stricmp(command.c_str(), "Stop")==0) {//Stop the server
			char buffer[1000] = "/kick all";
			tcp.sendPacketType(-1, buffer);
			if (serverstart) {
				if(tcp.closeConnection())
					serverstart = false;
				else 
					std::cout << "Server was unable to close" << std::endl;
			}
			else {
				std::cout << "Server is not running dickhead" << std::endl;
			}

		} else if (_stricmp(command.c_str(), "send") == 0) {//Broadcast a message to all the players
			//std::string message;
			//std::cin >> message;

			//another_console.Create("This is the first console");
		} else if (_stricmp(command.c_str(), "Player") == 0 || _stricmp(command.c_str(), "chat") == 0 
			|| _stricmp(command.c_str(), "connections") == 0 || _stricmp(command.c_str(), "server") == 0 || 
			_stricmp(command.c_str(), "broadcast") == 0) {//Broadcast a message to all the players
			logger appPlayer(command, true);	
		}
		else if (_stricmp(command.c_str(), "help") == 0) {
			std::cout << "=> Start : Will start the server if not running" << std::endl;
			std::cout << "=> Stop : Will stop the server if running" << std::endl;
			std::cout << "=> Send {player} : Will broadcast a message to the clients unless player is specified" << std::endl;
			std::cout << "=> {Player|Chat|connections|server}: shows logs being recorded for testing purpose and data usage (Player|Chat|connections|server)" << std::endl;
			std::cout << "=> / <warp x y> || <tp name or all name> || <kill name or all> || <kick name or all> " << std::endl;
		}
		else if (command.c_str()[0] == '/') {//Stop the server

			std::cout << "=> Commnad sent." << std::endl;
			char buffer[1000] = "";
			strcat_s(buffer, (char*)command.c_str());
			tcp.sendPacketType(-1, buffer);

		}
		else {
			std::cout << "=> Unknown command, try again." << std::endl;
		}
	}
	system("pause");
	return 0;

}
