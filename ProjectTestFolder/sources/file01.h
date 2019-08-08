/******************************************************************************
*
*   COMPANY:        NTT DATA Romania (19-21 Constanta Street, Cluj Napoca)
*
*   PROJECT:        2019 Summer Internship
*
*   FILE:           file01.h
*
*   AUTHOR:         John Doe
*
*   DESCRIPTION:	This is a demo file used by the C# Application
*
*   HISTORY:        14-Apr-2019 Initial Version
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
*   Full name:       requirementsTraceability
*
*   Description:     Search the requirement with "req" ID in all project files
*
*   Return Type:     none
*
*   Parameters:      req - the ID of the requirement
**************************************************************************** */
void requirementsTraceability(int &req);
void requirementsTraceability_copy(int &req);

	/* !LINKSTO NTT.RADU.SW.Req_201,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_202,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_203,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_204,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_205,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_206,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_207,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_208,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_209,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_210,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_211,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_212,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_213,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_214,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_215,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_216,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_217,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_218,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_219,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_220,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_221,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_222,1.1*/
	/* !LINKSTO NTT.RADU.SW.Req_223,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_224,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_225,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_226,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_227,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_228,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_229,1.0*/
	/* !LINKSTO NTT.RADU.SW.Req_243,1.0*/

/* EOF */
