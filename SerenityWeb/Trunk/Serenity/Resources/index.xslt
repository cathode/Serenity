<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output doctype-public="html" encoding="utf-8" method="xml" version="1.1" />
	<xsl:template match="/">
		<html xmlns='http://www.w3.org/1999/xhtml'>
			<head>
				<title>
					Index of <xsl:value-of select='index/meta/location'/>
				</title>
				<link rel='stylesheet' type='text/css' href='/resource/serenity/index.css' />
				<!--
				<xsl:for-each select='index/meta/stylesheet'>
					<link rel='stylesheet' type='text/css' href=''/>
				</xsl:for-each>
				-->
			</head>
			<body>
				<div class='main_heading'>
					Index of <xsl:value-of select='index/meta/location' />
				</div>
				<xsl:for-each select='index/group'>
					<div class='group_heading'>
						<xsl:value-of select='@name'/>
					</div>
					<table class='group'>
						<tr>
							<th class='icon' />
							<xsl:for-each select='column'>
								<th class=''>
									<xsl:value-of select='@name' />
								</th>
							</xsl:for-each>
						</tr>
						<xsl:for-each select='item'>
							<tr>
								<td>
									<img alt='x' class='icon' src='/resource/serenity/icons/page_white.png' />
								</td>
								<xsl:for-each select='value'>
									<td>
										<xsl:value-of select='.' />
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