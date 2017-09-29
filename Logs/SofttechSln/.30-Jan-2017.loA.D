17:26:00 - 1/30/2017 @@Debug: Login->SELECT U.USRID, EXPDT, UC.COMPC, UB.BRNCH,
                 SAL_CODENAME('COMPC',UC.COMPC,NULL) AS COMPANY_NAME, SAL_CODENAME('BRNCH',UB.BRNCH,NULL) AS BRANCH_NAME
                 FROM SEC_USERNAME U                 
                 LEFT JOIN SEC_USERCMPN UC
                      ON U.USRID = UC.USRID 
                 LEFT JOIN SEC_USERBRCH UB
                      ON U.USRID = UB.USRID 
                 WHERE UPPER(U.DESCR) = 'FOURGEN ADMIN' 
                    AND U.PASWD = (SELECT COM_BALANCE.PayableZ(UPPER('managerits') || U.USRID || U.DESCR) FROM DUAL)

17:28:55 - 1/30/2017 @@Debug: Login->SELECT U.USRID, EXPDT, UC.COMPC, UB.BRNCH,
                 SAL_CODENAME('COMPC',UC.COMPC,NULL) AS COMPANY_NAME, SAL_CODENAME('BRNCH',UB.BRNCH,NULL) AS BRANCH_NAME
                 FROM SEC_USERNAME U                 
                 LEFT JOIN SEC_USERCMPN UC
                      ON U.USRID = UC.USRID 
                 LEFT JOIN SEC_USERBRCH UB
                      ON U.USRID = UB.USRID 
                 WHERE UPPER(U.DESCR) = 'FOURGEN ADMIN' 
                    AND U.PASWD = (SELECT COM_BALANCE.PayableZ(UPPER('managerits') || U.USRID || U.DESCR) FROM DUAL)

17:30:45 - 1/30/2017 @@Debug: Login->SELECT U.USRID, EXPDT, UC.COMPC, UB.BRNCH,
                 SAL_CODENAME('COMPC',UC.COMPC,NULL) AS COMPANY_NAME, SAL_CODENAME('BRNCH',UB.BRNCH,NULL) AS BRANCH_NAME
                 FROM SEC_USERNAME U                 
                 LEFT JOIN SEC_USERCMPN UC
                      ON U.USRID = UC.USRID 
                 LEFT JOIN SEC_USERBRCH UB
                      ON U.USRID = UB.USRID 
                 WHERE UPPER(U.DESCR) = 'FOURGEN ADMIN' 
                    AND U.PASWD = (SELECT COM_BALANCE.PayableZ(UPPER('managerits') || U.USRID || U.DESCR) FROM DUAL)

