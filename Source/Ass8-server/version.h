#ifndef VERSION_H
#define VERSION_H

namespace AutoVersion{
	
	//Date Version Types
	static const char DATE[] = "28";
	static const char MONTH[] = "05";
	static const char YEAR[] = "2009";
	static const double UBUNTU_VERSION_STYLE = 9.05;
	
	//Software Status
	static const char STATUS[] = "Beta";
	static const char STATUS_SHORT[] = "b";
	
	//Standard Version Type
	static const long MAJOR = 0;
	static const long MINOR = 3;
	static const long BUILD = 18;
	static const long REVISION = 125;
	
	//Miscellaneous Version Types
	static const long BUILDS_COUNT = 320;
	#define RC_FILEVERSION 0,3,18,125
	#define RC_FILEVERSION_STRING "0, 3, 18, 125\0"
	static const char FULLVERSION_STRING[] = "0.3.18.125";
	
	//These values are to keep track of your versioning state, don't modify them.
	static const long BUILD_HISTORY = 2;
	

}
#endif //VERSION_H
