DELIMITER //
CREATE FUNCTION BinaryIdToUrlId(
    binaryId BINARY(16)
) RETURNS CHAR(22) DETERMINISTIC
BEGIN
    DECLARE intermediate VARCHAR(50);
    SET intermediate = TO_BASE64(binaryId);
    SET intermediate = REPLACE(intermediate,'+','-');
    SET intermediate = REPLACE(intermediate,'/','_');
    RETURN SUBSTRING(intermediate,1,22);
END//
DELIMITER ;

DELIMITER //
CREATE FUNCTION UrlIdToBinaryId(
    urlId CHAR(22)
) RETURNS BINARY(16) DETERMINISTIC
BEGIN
    SET urlId = REPLACE(urlId,'_','/');
    SET urlId = REPLACE(urlId,'-','+');
    RETURN FROM_BASE64(CONCAT(urlId,'=='));
END//
DELIMITER ;
