#ifndef VERSION_H
#define VERSION_H

namespace AutoVersion{
	
	//Date Version Types
	static const char DATE[] = "01";
	static const char MONTH[] = "06";
	static const char YEAR[] = "2009";
	static const double UBUNTU_VERSION_STYLE = 9.06;
	
	//Software Status
	static const char STATUS[] = "Beta";
	static const char STATUS_SHORT[] = "b";
	
	//Standard Version Type
	static const long MAJOR = 0;
	static const long MINOR = 7;
	static const long BUILD = 2;
	static const long REVISION = 143;
	
	//Miscellaneous Version Types
	static const long BUILDS_COUNT = 344;
	#define RC_FILEVERSION 0,7,2,143
	#define RC_FILEVERSION_STRING "0, 7, 2, 143\0"
	static const char FULLVERSION_STRING[] = "0.7.2.143";
	
	//These values are to keep track of your versioning state, don't modify them.
	static const long BUILD_HISTORY = 0;
	

}
#endif //VERSION_H
