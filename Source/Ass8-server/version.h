#ifndef VERSION_H
#define VERSION_H

namespace AutoVersion{
	
	//Date Version Types
	static const char DATE[] = "07";
	static const char MONTH[] = "05";
	static const char YEAR[] = "2009";
	static const double UBUNTU_VERSION_STYLE = 9.05;
	
	//Software Status
	static const char STATUS[] = "Alpha";
	static const char STATUS_SHORT[] = "a";
	
	//Standard Version Type
	static const long MAJOR = 0;
	static const long MINOR = 0;
	static const long BUILD = 8;
	static const long REVISION = 54;
	
	//Miscellaneous Version Types
	static const long BUILDS_COUNT = 208;
	#define RC_FILEVERSION 0,0,8,54
	#define RC_FILEVERSION_STRING "0, 0, 8, 54\0"
	static const char FULLVERSION_STRING[] = "0.0.8.54";
	
	//These values are to keep track of your versioning state, don't modify them.
	static const long BUILD_HISTORY = 8;
	

}
#endif //VERSION_H
