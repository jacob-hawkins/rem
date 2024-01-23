# Rem

# Documentation

## Database

The database was created using Postgres and will hopefully be hosted on Vercel soon. It is comprised of 3 tables.

### 1. user

| user_id | username | password | email             | completed_reminders | total_reminders |
| ------- | -------- | -------- | ----------------- | ------------------- | --------------- |
| 1       | example  | password | example@email.com | 2                   | 6               |

`user_id`: A unique id for each user. This connects the user to their reminders.

`username`: The users username.

`password`: The users password.

`email`: The users email.

`completed_reminders`: The total number of reminders the user has completed during the lifetime of the account.

`total_reminders`: The total number of reminders the user has created during the lifetime of the account. This includes completed and deleted reminders.

### 2. reminders

| reminder_id | user_id | title | date | completed |
| ----------- | ------- | ----- | ---- | --------- |
| 1            | 1        | Example reminder      | 2024-01-01     | false          |

`reminder_id`: A unique id for each reminder. This connects the reminders to the `work_on_dates` table.

`user_id`: A unique id for each user. This connects the user to their reminders.

`title`: The title of the reminder.

`date`: The date of the reminder.

`completed`: Status of the reminder (completed/incompleted)

### 3. work_on_dates

| reminder_id | date1 | date2 | date3 |
| ----------- | ----- | ----- | ----- |
| 1           | 2024-01-01      | 2024-01-02      | 2024-01-03      |

`reminder_id`: A unique id for each reminder. Connecting the reminder to the list of dates.

`date`: List of dates connecting to the reminders for additional working on dates.

## Command Line Tool

### Usage

| Command | Description                                                                                 |
| ------- | ------------------------------------------------------------------------------------------- |
| init    | Initialize reminder system files. (This only needs to be run once)                          |
| add     | Add a reminder to list. You will be prompted for a title, date, and optional 'work on days' |
