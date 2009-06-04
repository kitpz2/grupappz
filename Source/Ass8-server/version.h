#ifndef VERSION_H
#define VERSION_H

namespace AutoVersion{
	
	//Date Version Types
	static const char DATE[] = "04";
	static const char MONTH[] = "06";
	static const char YEAR[] = "2009";
	static const double UBUNTU_VERSION_STYLE = 9.06;
	
	//Software Status
	static const char STATUS[] = "Release Candidate";
	static const char STATUS_SHORT[] = "rc";
	
	//Standard Version Type
	static const long MAJOR = 0;
	static const long MINOR = 9;
	static const long BUILD = 0;
	static const long REVISION = 144;
	
	//Miscellaneous Version Types
	static const long BUILDS_COUNT = 364;
	#define RC_FILEVERSION 0,9,0,144
	#define RC_FILEVERSION_STRING "0, 9, 0, 144\0"
	static const char FULLVERSION_STRING[] = "0.9.0.144";
	
	//These values are to keep track of your versioning state, don't modify them.
	static const long BUILD_HISTORY = 1;
	

}
#endif //VERSION_H
