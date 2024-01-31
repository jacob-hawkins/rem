# Rem

# Documentation

# Database

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

| reminder_id | user_id | title            | date       | completed |
| ----------- | ------- | ---------------- | ---------- | --------- |
| 1           | 1       | Example reminder | 2024-01-01 | false     |

`reminder_id`: A unique id for each reminder. This connects the reminders to the `work_on_dates` table.

`user_id`: A unique id for each user. This connects the user to their reminders.

`title`: The title of the reminder.

`date`: The date of the reminder.

`completed`: Status of the reminder (completed/incompleted)

### 3. work_on_dates

| reminder_id | date1      | date2      | date3      |
| ----------- | ---------- | ---------- | ---------- |
| 1           | 2024-01-01 | 2024-01-02 | 2024-01-03 |

`reminder_id`: A unique id for each reminder. Connecting the reminder to the list of dates.

`date`: List of dates connecting to the reminders for additional working on dates.

# Command Line Tool

## Usage

| Command | Description                                                                                  |
| ------- | -------------------------------------------------------------------------------------------- |
| init    | Initialize reminder system files. (This only needs to be run once)                           |
| add     | Add a reminder to list. You will be prompted for a title, date, and optional 'work on days'. |
| view    | Print your reminders.                                                                        |
| help    | Displays all commands and descriptions available.                                            |

## `init`

## `add`

The user will be prompted to enter reminder. If the user uses the keyword `on` followed by the date of when the reminder should be, the reminder will be added to that day. You may use semantic dates (_today_, _Monday_, _Tuesday_, etc.) or exact dates (_1/1/24_, etc.) when entering the date. If the keyword `on` is not used, the user will be prompted to insert a date. If the user decides to cancel adding the reminder, simply press enter with a blank prompt. If on the beginning reminder prompt, the task will exit. If on the secondary date prompt, the user will be prompted again saying _"✘ You must enter a date (Press enter again to cancel add)."_. Press enter again and the task will be exited **without** adding the reminder. If the reminder was successfully added to your reminder list, a success message reading _"✔ Successfully added reminder to list."_ will be displayed. If any errors occur, the user with be notified accordingly.

**Work on Days Coming Soon!**

## `view`

Your reminder list will be displayed. At the top, it displays the current day of the week and the date. Below the line, each day of the current week will be displayed.

#### Categories

If there are any overdue reminders from previous week, there will be a _0 Past_ section displayed. If there are any reminders in the future from the current week, there will be a _8 Future_ section displayed. Each day of the week has a number next to the name (i.e. _1 Sunday_).

#### Reminder Fields

Each reminder is categorized under the day that the reminder is due and displays four fields.

`1` Example Reminder &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; 1/1/2024 &ensp; [ ]

> This number signifies a unique identifier for each day. This is used when marking reminders complete (see `complete`).

1 `Example Reminder` &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; 1/1/2024 &ensp; [ ]

> The title of the reminder.

1 Example Reminder &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; `1/1/2024` &ensp; [ ]

> The date the reminder is due. This value determines where the reminder is categorized.

1 Example Reminder &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; &emsp; 1/1/2024 &ensp; `[ ]`

> Status of the reminder. If the reminder is incomplete this will display as [ ]. If the reminder has been completed, this will display as [x] and will be greyed out.

#### Coloring

The heading (containing the current day and the week and date) as well as the current day in the week in the section below are colored yellow signifying it is the current day. The rest of the days in the week are colored blue as headings.

The reminders can be three colors: white, grey, or red. A white reminder is a reminder that is currently active and not overdue. A red reminder shows that the reminder is overdue. A grey reminder is a reminder that has been marked completed. Any completed reminders will be deleted automatically when they are past the current week.
