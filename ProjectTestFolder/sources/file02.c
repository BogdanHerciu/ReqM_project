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
    /* !LINKSTO NTT.RADU.SW.Req002,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req003,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req004,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req005,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req006,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req007,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req008,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req009,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req010,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req011,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req012,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req013,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req014,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req015,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req016,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req017,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req018,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req019,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req020,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req021,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req022,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req023,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req024,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req025,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req026,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req027,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req028,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req029,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req030,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req031,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req032,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req033,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req034,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req035,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req036,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req037,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req038,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req039,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req040,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req041,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req042,1.0*/
    /* !LINKSTO NTT.RADU.SW.Req043,1.0*/
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