/******************************************************************************
*
*   COMPANY:        NTT DATA Romania (19-21 Constanta Street, Cluj Napoca)
*
*	PROJECT:		2019 Summer Internship
*
*	FILE:			file02.c
*
*	AUTHOR:			John Doe
*
*	DESCRIPTION:	This is a demo file used by the C# Application
*
*	HISTORY:        14-Apr-2019 Initial Version
*
**************************************************************************** */

/* include files *********************************************************** */
#include <iostream.h>
#include <stdio.h>
#include "file01.h"
#include "file02.h"
#include "file03.h"

/*  global declarations **************************************************** */
#ifdef (APPLICATION_STATUS)
volatile int ReadSettings = TRUE;
#endif

/* constant definitions **************************************************** */
#ifndef COLUMN_1
  #define COLUMN_1 1
#endif /* COLUMN_1 */
#ifndef COLUMN_2
  #define COLUMN_2 2
#endif /* COLUMN_2 */
#ifndef COLUMN_3
  #define COLUMN_3 3
#endif /* COLUMN_3 */
#ifndef COLUMN_4
  #define COLUMN_4 4
#endif /* COLUMN_4 */
#ifndef COLUMN_5
  #define COLUMN_5 5
#endif /* COLUMN_5 */
#ifndef MAX_COLUMNS
#define MAX_COLUMNS 6
#endif /* COLUMN_5 */

volatile uint8_t variable1;
volatile uint32_t ClujNapoca;

/* type definitions ******************************************************** */
enum ListOfRequirements
{
	reqA = 100,
	reqB,
	reqC,
	reqD,
};

/* functions ************************************************************** */

/******************************************************************************
*	Full name:       main
*
*   Description:     main function
*
*   Return Type:     none
*
*   Parameters:      none
**************************************************************************** */
void main(void)
{
    /* !LINKSTO NTT.RADU.SW.Req_002,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_003,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_004,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_005,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_006,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_007,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_008,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_009,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_010,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_011,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_012,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_013,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_014,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_015,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_016,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_017,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_018,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_019,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_020,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_021,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_022,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_023,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_024,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_025,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_039,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_040,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_041,1.1*/
    /* !LINKSTO NTT.RADU.SW.Req_042,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req_043,1.0*/
    /* requirement_implementation: REQ1 */
    int index = 0;
    for (index = 3; index < MAX_COLUMNS; index++)
    {
        /* requirement_implementation: REQ2 */
        cout << "NTT DATA Romania" << endl;
    }

    /* requirement_implementation: REQ3 */
    printf("End of program\n");
}

/* EOF */