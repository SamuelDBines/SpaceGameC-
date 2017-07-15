#include "loggerHeader.h"


logger::logger(std::string file, bool view) 
{
	strcpy_s(filename, file.c_str());
	if (_stricmp(file.c_str(), "player") == 0) {
		strcpy_s(filename, "PlayerLog.txt");
	}
	else if (_stricmp(file.c_str(), "chat" ) == 0) {
		strcpy_s(filename, "ChatLog.txt");
	}
	else if (_stricmp(file.c_str(), "connections") == 0) {
		strcpy_s(filename, "ConnectionsLog.txt");
	}
	else if (_stricmp(file.c_str(), "Server") == 0) {
		strcpy_s(filename, "ServerLog.txt");		
	} 
	else if (_stricmp(file.c_str(), "BroadCast") == 0) {
		strcpy_s(filename, "BroadCastLog.txt");
	}
	else {
		std::cout << "=> Unknown command, try again." << std::endl;
	}
	if(view == true)
		viewLog();
}
logger::~logger() {
	file.close();
}
bool logger::appendPlayerLog(bool connected, int userID, std::string username, std::string ipAddress) {
	try {
		file.open(filename, ios::app);
		file << "------------------------------------------" << endl;
		file << "Updated :" << currentDateTime() << endl;
		file << "Connected:" << connected << endl;
		file << "User ID:" << userID << endl;
		file << "User name:" << username << endl;
		file << "IP Address :" << ipAddress << endl;		
		file << "------------------------------------------" << endl;
		file.close();
		return true;
	}
	catch (...) {
		return false;
	}
}
bool logger::appendServerLog(std::string ipAddress, std::string gateway, int PORT) {
	try {
		file.open(filename, ios::app);
		file << "------------------------------------------" << endl;
		file << "Updated :" << currentDateTime() << endl;
		file << "IP Address :" << ipAddress << endl;
		file << "gateway :" << gateway << endl;
		file << "------------------------------------------" << endl;
		file.close();
		return true;
	}
	catch (...) {
		return false;
	}	
}
bool logger::appendChatLog(int userID, std::string username, std::string chat)
{
	try {
		file.open(filename, ios::app);
		file << "------------------------------------------" << endl;
		file << "Updated :" << currentDateTime() << endl;
		file << "Player ID :" << userID << endl;
		file << "Username:" << username << endl;
		file << "Player ID :" << chat << endl;
		file << "------------------------------------------" << endl;
		file.close();
		return true;
	}
	catch (...) {
		return false;
	}
}
void logger::viewLog() {
	std::string line;
	file.open(filename);
	if (file.is_open())
	{
		while (getline(file, line))
		{
			cout << line << '\n';
		}
		file.close();
	}
}
std::string logger::currentDateTime() {
	time_t     now = time(0);
	struct tm  tstruct;
	char       buf[80];
	tstruct = *localtime(&now);
	strftime(buf, sizeof(buf), "%Y-%m-%d.%X", &tstruct);
	return buf;
}