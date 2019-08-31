DELIMITER //
CREATE PROCEDURE GetPublisherPopularTracks(
    IN urlAlias VARCHAR(128),
    IN limitNum INT,
    IN offsetNum INT
)
BEGIN 
    SELECT track_name,artist_name,publisher_name,count,on_spotify,on_youtube,on_soundcloud,on_apple_music
    FROM track t 
    INNER JOIN (
        SELECT track_id,COUNT(*) as count
        FROM track_by_setlist tbs
        GROUP BY track_id
        ORDER BY count
    ) t2 ON t.id = t2.track_id
    WHERE publisher_name=GetPublisherNameByURLAlias(publisherName)
    GROUP BY id
    ORDER BY count DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ; 

DELIMITER //
CREATE FUNCTION GetPublisherNameByURLAlias(
    urlAlias VARCHAR(64)
) RETURNS VARCHAR(64) DETERMINISTIC
BEGIN
    RETURN (
        REPLACE(urlAlias, '-', ' ')
    );
END//
DELIMITER ;
