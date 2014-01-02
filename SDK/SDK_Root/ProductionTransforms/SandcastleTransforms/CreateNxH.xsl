<?xml version="1.0"?>
<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.1">

  <!-- Create HxC project file for hxcomp.exe. The input can be any well formed
       XML file; it is used only to satisfy XslTransform, which needs an input
       file to run the transform. Make sure to set the fileNamePrefix. -->

  <!-- $fileNamePrefix is the prefix used for all files names. -->
  <xsl:param name="fileNamePrefix">test</xsl:param>

  <xsl:template match="/">
    <HelpCollection Id="{$fileNamePrefix}" Title="{$fileNamePrefix}" FileVersion="1.0" ShortDescription="" Copyright="" LangID="1033"
                    xmlns="http://schemas.nasutek.com/2013/Help5/Help42Extensions">
      <CompileDirective FilePath="{$fileNamePrefix}.NxS">
        <Include Path="html" />
        <Include Path="icons" />
        <Include Path="scripts" />
        <Include Path="styles" />
        <Include Path="{$fileNamePrefix}.NxT" />
      </CompileDirective>

      <TOCDef File="{$fileNamePrefix}.NxT" />
    </HelpCollection>
  </xsl:template>

</xsl:stylesheet>