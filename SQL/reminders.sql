-- DROP TABLE work_on;
DROP TABLE reminders;

CREATE TABLE reminders (
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	user_id BIGINT NOT NULL,
	title VARCHAR(50) NOT NULL,
	date DATE NOT NULL,
	completed BOOL DEFAULT False,
	work_on_count INT DEFAULT 0,
	work_on BOOL DEFAULT False,
	work_on_reminder BIGINT DEFAULT -1
);

-- CREATE TABLE work_on (
-- 	id BIGSERIAL NOT NULL PRIMARY KEY,
-- 	reminder_id BIGINT REFERENCES reminders(id), -- FORIEGN KEY
-- 	title VARCHAR(50) NOT NULL,
-- 	date DATE NOT NULL,
-- 	completed BOOL DEFAULT False
-- );

INSERT INTO reminders (user_id, title, date) VALUES (1, 'test past', '5/14/24');
INSERT INTO reminders (user_id, title, date) VALUES (1, 'test', '5/16/24');
INSERT INTO reminders (user_id, title, date, completed) VALUES (1, 'completed', '5/16/24', True);

INSERT INTO reminders (user_id, title, date, work_on, work_on_reminder) VALUES (1, 'WORK ON TEST', '5/17/24', True, 1);
UPDATE reminders SET work_on_count=work_on_count+1 WHERE id = 2;

INSERT INTO reminders (user_id, title, date, work_on, work_on_reminder) VALUES (1, 'WORK ON TEST 2', '5/18/24', True, 1);
UPDATE reminders SET work_on_count=work_on_count+1 WHERE id = 2;

SELECT * FROM reminders;
-- SELECT * FROM work_on;
