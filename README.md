# Rem

# Documentation

## Database

The database was created using Postgres and will hopefully be hosted on Vercel soon. It is comprised of 3 tables.

### 1. User

| user_id | username | password | email             | completed_reminders | total_reminders |
| ------- | -------- | -------- | ----------------- | ------------------- | --------------- |
| 1       | example  | password | example@email.com | 2                   | 6               |

`user_id`: A unique id for each user. This connects the user to their reminders.
`username`: The users username.
`password`: The users password.
`email`: The users email.
`completed_reminders`: The total number of reminders the user has completed during the lifetime of the account.
`total_reminders`: The total number of reminders the user has created during the lifetime of the account. This includes completed and deleted reminders.

## Command Line Tool

### Usage

| Command | Description                                                                                 |
| ------- | ------------------------------------------------------------------------------------------- |
| init    | Initialize reminder system files. (This only needs to be run once)                          |
| add     | Add a reminder to list. You will be prompted for a title, date, and optional 'work on days' |
