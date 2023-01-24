# TodoListTasks

At the frontend, use `npm i --force` instead of `npm i` while installing the packages.

- This is a bare minimum project, so it differs significantly from the production-ready quality and practices. One such example is that it does not have any backend and the frontend tests and one more of such example is the missing confirmation dialog while deleting in the frontend UI. 

- Users are hardcoded and the valid usernames can be seen in the login page.

- Todo items are stored using the mongoDB database.

- Token authentication is added but the infrastructure to renew the refresh token is not in place.

- For the sake of convenience, token expiration duration is set for 1 day.
