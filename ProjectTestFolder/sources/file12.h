/******************************************************************************
*
*   COMPANY:        NTT DATA Romania (19-21 Constanta Street, Cluj Napoca)
*
*	PROJECT:		2019 Summer Internship
*
*	FILE:			file12.h
*
*	AUTHOR:			John Doe
*
*	DESCRIPTION:	This is a demo file used by the C# Application
*
*	HISTORY:        14-Apr-2019 Initial Version
*
**************************************************************************** */

/* include files *********************************************************** */
#include <stdio.h>

/*  global declarations **************************************************** */
#ifdef (APPLICATION_STATUS)
volatile int ReadSettings = TRUE;
#endif
volatile char requirementID;
volatile long ClujNapoca;

/* constant definitions **************************************************** */
#define COLUMN_1 1
#define COLUMN_2 2
#define COLUMN_3 3
#define COLUMN_4 4
#define COLUMN_5 5

/* type definitions ******************************************************** */
enum RequirementList
{
	reqA = 0,
	reqB,
	reqC,
	reqD,
};

/* functions ************************************************************** */

/******************************************************************************
*	Full name:       requirementsTraceability
*
*   Description:     Search the requirement with "req" ID in all project files
*
*   Return Type:     none
*
*   Parameters:      req - the ID of the requirement
**************************************************************************** */
void requirementsTraceability(int &req);

/* EOF */
