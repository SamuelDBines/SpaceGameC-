#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <ctime>
#include <iostream>
#include <fstream>
#include <string>
using namespace std;
class logger {
public:
	logger(std::string file, bool view = false);
	~logger();
	bool appendChatLog(int userID, std::string username,std::string chat);
	bool appendPlayerLog(bool conntected, int userID, std::string username, std::string ipAddress);
	bool appendServerLog(std::string ipAddress, std::string gateway, int PORT);
	std::string currentDateTime();
	void viewLog();
private:
	char filename[20];
	fstream file;

};

