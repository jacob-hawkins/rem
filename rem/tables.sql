-- DELETE FROM reminders WHERE title = 'test ';

-- DROP TABLE users;
DROP TABLE work_on;
DROP TABLE reminders;

CREATE TABLE reminders (
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	user_id BIGINT NOT NULL,
	title VARCHAR(50) NOT NULL,
	date DATE NOT NULL,
	completed BOOL DEFAULT False,
	work_on INT DEFAULT 0
);

CREATE TABLE work_on (
	id BIGSERIAL NOT NULL PRIMARY KEY,
	reminder_id BIGINT REFERENCES reminders(id), -- FORIEGN KEY
	title VARCHAR(50) NOT NULL,
	date DATE NOT NULL,
	completed BOOL DEFAULT False
);

INSERT INTO reminders (user_id, title, date) VALUES (1, 'test past', '5/14/24');
INSERT INTO reminders (user_id, title, date) VALUES (1, 'test', '5/16/24');

INSERT INTO work_on (reminder_id, title, date) VALUES (1, 'WORK ON TEST', '5/17/24');
UPDATE reminders SET work_on=work_on+1 WHERE id = 1;

INSERT INTO work_on (reminder_id, title, date) VALUES (1, 'WORK ON TEST 2', '5/18/24');
UPDATE reminders SET work_on=work_on+1 WHERE id = 1;

-- SELECT * FROM users;
SELECT * FROM reminders;
-- SELECT * FROM work_on;