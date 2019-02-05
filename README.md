# QuoteCrawler
Crawler for obtaining quotes from the following site: https://citatum.hu/

Have you ever needed hungarian Paulo Coelho or Nora Oravecz quotes for an urgent matter?
Well, I have, so I made this little tool for myself which crawls quotes from the aforementioned site, and pushes them to a [Firebase database](https://firebase.google.com/docs/database/).

Arguments:
- First: Firebase Database URL
- Second: Maximum Quote Length
- Third: Author names separated with commas

Example:
```
dotnet run "https://mydatabase-666.firebaseio.com/" 200 "Paulo Coelho,Oravecz Nora"
```
