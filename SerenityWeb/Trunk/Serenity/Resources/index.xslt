<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output doctype-public="html" encoding="utf-8" method="xml" version="1.1" />
	<xsl:template match="/">
		<html xmlns='http://www.w3.org/1999/xhtml'>
			<head>
				<title>
					Index of <xsl:value-of select='index/location' />
				</title>
				<link rel='stylesheet' type='text/css' href='/resource/serenity/index.css' />
				<xsl:for-each select='index/presentation/stylesheet'>
					<xsl:element name='link'>
						<xsl:attribute name='rel'>stylesheet</xsl:attribute>
						<xsl:attribute name='type'>text/css</xsl:attribute>
						<xsl:attribute name='href'>
							<xsl:value-of select='.' />
						</xsl:attribute>
					</xsl:element>
				</xsl:for-each>
			</head>
			<body>
				<div class='main_heading'>
					Index of <xsl:value-of select='index/location' />
				</div>
				<xsl:for-each select='index/group'>
					<div class='group_heading'>
						<xsl:value-of select='@name'/>
					</div>
					<table class='group'>
						<tr>
							<th class='icon' />
							<xsl:for-each select='field'>
								<xsl:element name='th'>
									<xsl:attribute name='class'>
										<xsl:value-of select='@id' />
									</xsl:attribute>
									<xsl:value-of select='@name' />
								</xsl:element>
							</xsl:for-each>
						</tr>
						<xsl:for-each select='item'>
							<tr>
								<td>
									<xsl:element name='img'>
										<xsl:attribute name='alt'>x</xsl:attribute>
										<xsl:attribute name='src'>/resource/serenity/icons/<xsl:value-of select='@icon' />.png</xsl:attribute>
									</xsl:element>
								</td>
								<xsl:for-each select='value'>
									<td>
										<xsl:choose>
										<xsl:when test='@link'>
											<xsl:element name='a'>
												<xsl:attribute name='href'><xsl:value-of select='@link' /></xsl:attribute>
												<xsl:value-of select='.' />
											</xsl:element>
										</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select='.' />
											</xsl:otherwise>
										</xsl:choose>
									</td>
								</xsl:for-each>
							</tr>
						</xsl:for-each>
					</table>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>