echo "convert %1i18n\common\messages.txt"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\resgen.exe" "%1i18n\common\messages.txt" "%1i18n\common\messages.resx" /str:cs,Translations.i18n.common,messages,"%1i18n\common\messages.Designer.cs" /publicclass

echo "convert %1i18n\modules\core\accounts\controller\messages.txt"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\resgen.exe" "%1i18n\modules\core\accounts\controller\messages.txt" "%1i18n\modules\core\accounts\controller\messages.resx" /str:cs,Translations.i18n.modules.core.accounts.controller,messages,"%1i18n\modules\core\accounts\controller\messages.Designer.cs" /publicclass

echo "convert %1i18n\modules\site\home\controller\messages.txt"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\resgen.exe" "%1i18n\modules\site\home\controller\messages.txt" "%1i18n\modules\site\home\controller\messages.resx" /str:cs,Translations.i18n.modules.site.home.controller,messages,"%1i18n\modules\site\home\controller\messages.Designer.cs" /publicclass

echo "convert %1i18n\modules\core\accounts\login\messages.txt"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\resgen.exe" "%1i18n\modules\core\accounts\login\messages.txt" "%1i18n\modules\core\accounts\login\messages.resx" /str:cs,Translations.i18n.modules.core.accounts.login,messages,"%1i18n\modules\core\accounts\login\messages.Designer.cs" /publicclass

echo "convert %1i18n\modules\site\home\index\messages.txt"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\resgen.exe" "%1i18n\modules\site\home\index\messages.txt" "%1i18n\modules\site\home\index\messages.resx" /str:cs,Translations.i18n.modules.site.home.index,messages,"%1i18n\modules\site\home\index\messages.Designer.cs" /publicclass