DELIMITER //
CREATE PROCEDURE GetArtistPopularTracks(
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
    WHERE artist_name=GetArtistNameByURLAlias(urlAlias)
    GROUP BY id
    ORDER BY count DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ; 

DELIMITER //
CREATE PROCEDURE GetArtistTracksInLastNSets(
    IN artistName VARCHAR(128),
    IN numSets INT,
    IN limitNum INT,
    IN offsetNum INT 
)
BEGIN 
    SELECT track_name,artist_name,publisher_name,count,on_spotify,on_youtube,on_soundcloud,on_apple_music
    FROM track t 
    INNER JOIN (
        SELECT track_id,COUNT(*) as count
        FROM track_by_setlist tbs
        INNER JOIN (
            SELECT id
            FROM setlist
            WHERE artist_name=GetArtistNameByURLAlias(artistName)
            LIMIT numSets
        ) s ON s.id = tbs.set_id
        GROUP BY track_id
        ORDER BY count
    ) t2 ON t.id = t2.track_id
    GROUP BY id
    ORDER BY count DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE GetArtistTracksInSetsInDateRange(
    IN artistName VARCHAR(128),
    IN startDate DATE,
    IN endDate DATE,
    IN limitNum INT,
    IN offsetNum INT
)
BEGIN 
    SELECT track_name,artist_name,publisher_name,count,on_spotify,on_youtube,on_soundcloud,on_apple_music
    FROM track t 
    INNER JOIN (
        SELECT track_id,COUNT(*) as count
        FROM track_by_setlist tbs
        INNER JOIN (
            SELECT id
            FROM setlist
            WHERE artist_name=GetArtistNameByURLAlias(artistName) AND startDate <= set_date AND set_date <= endDate
        ) s ON s.id = tbs.set_id
        GROUP BY track_id
        ORDER BY count
    ) t2 ON t.id = t2.track_id
    GROUP BY id
    ORDER BY count DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE GetArtistSetsInDateRange(
    IN artistName VARCHAR(128),
    IN startDate DATE,
    IN endDate DATE,
    IN limitNum INT,
    IN offsetNum INT
)
BEGIN 
    SELECT set_name,artist_name,venue_event,set_date,thou_tl_uri,BinaryIdToUrlId(id) as uri
    FROM setlist
    WHERE artist_name=GetArtistNameByURLAlias(artistName) AND startDate <= set_date AND set_date <= endDate
    ORDER BY set_date DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE GetArtistSets(
    IN artistName VARCHAR(128),
    IN limitNum INT,
    IN offsetNum INT
)
BEGIN 
    SELECT set_name,artist_name,venue_event,set_date,thou_tl_uri,BinaryIdToUrlId(id) as uri
    FROM setlist
    WHERE artist_name=GetArtistNameByURLAlias(artistName) 
    ORDER BY set_date DESC
    LIMIT limitNum
    OFFSET offsetNum;
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE GetArtistSetListTracksByName(
    IN artistName VARCHAR(128),
    IN setName VARCHAR(128)
)
BEGIN 
    SELECT track_name,artist_name,publisher_name,idx,on_spotify,on_youtube,on_soundcloud,on_apple_music
    FROM track t 
    INNER JOIN (
        SELECT set_id,track_id,idx 
        FROM track_by_setlist
        WHERE set_id=(
            SELECT id 
            FROM setlist
            WHERE artist_name=artistName AND set_name LIKE setName
            ORDER BY set_date DESC
            LIMIT 1
        )
    ) tbs ON t.id=tbs.track_id
    ORDER BY idx ASC;
END//
DELIMITER ;

DELIMITER //
CREATE FUNCTION GetArtistNameByURLAlias(
    urlAlias VARCHAR(64)
) RETURNS VARCHAR(64) DETERMINISTIC
BEGIN 
    RETURN ( 
        SELECT artist_name 
        FROM artist a INNER JOIN (
            SELECT artist_id 
            FROM artist_by_urlalias 
            WHERE url_alias=urlAlias
        ) u ON a.id=u.artist_id
    );
END//
DELIMITER ;

