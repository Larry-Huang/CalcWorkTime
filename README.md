# Calculate the employee's work time  in Industry unit.
 
## Timespan Table
TimeSpan|act time|
--------|:-------
10:00-07:50|130		
12:00-10:10|110
15:00-12:50|130
18:00-15:10|170						

Q:assume check in time is 09:00 , check out time is 1730
---	
The main logic is to use the front-to-back intersection method to do a one-time calculation. The result is called the total work, 
and the final result is obtained. For example
Calculation method: (0900-1000) + A (10:00 to 1800 working hours) + (1510-1700) + B (07:50 to 1510 working hours)-C (total working hours between 0750-1800)
=> 60 (0900-1000) + A (410) + 110 (1510-1700) + B (370)-C (540) = 410 actual working hours 410 minutes.						
												
												
												
												
												
												
												
