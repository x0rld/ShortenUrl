CREATE TABLE IF NOT EXISTS "storedUrl" 
    (
    id        text
        primary key,
    website   text,
    createdOn INT default CURRENT_TIMESTAMP
)