DROP TABLE users;

CREATE TABLE users (
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	username VARCHAR(50) NOT NULL UNIQUE,
	password VARCHAR(50) NOT NULL,
	email VARCHAR(200) NOT NULL,
	completed_reminders INT DEFAULT 0,
	total_reminders INT DEFAULT 0,
	admin bool DEFAULT false,
	notion_key VARCHAR(50) DEFAULT -1
);

INSERT INTO users (username, password, email, admin) VALUES ('jacobhawkins', 'password', 'jacob.hawkins010@gmail.com', true);

SELECT * FROM users;