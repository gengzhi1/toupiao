const fs = require("fs");
const path = require("path");

const readline = require('readline');

var message_data = (name)=>`

  <data name="${name}" xml:space="preserve">
    <value></value>
  </data>

`;

const get_all_files = function ( dirPath, arrayOfFiles) {
    files = fs.readdirSync(dirPath);

    arrayOfFiles = arrayOfFiles || [];

    files.forEach(function (file) {
        
        if (fs.statSync(dirPath + "/" + file).isDirectory()) {
            if( file=='bin'||file=='obj' ) return;
            arrayOfFiles = get_all_files(dirPath + "/" + file, arrayOfFiles);
        }
        else
        {
            arrayOfFiles.push(path.join(__dirname, dirPath, "/", file));
        }
    })

    return arrayOfFiles;
} 


const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.question(`Where to read files?('./') `, ( project_path ) => {

    project_path = project_path || './';

    rl.question(
        `Which directory your .resx files in?('./Localization') `,
        (resx_path) => {
            resx_path = resx_path || './Localization';

            var all_files = get_all_files(project_path)
                .filter((value,index, array)=>{
                    console.log( value );
                    return value.endsWith('.cs')||value.endsWith('.cdhtml');
                });

            console.log(all_files);

            var all_names = [];
            var message_reg = /(\@Localizer\["(\S+)"\]|_localizer\["(\S+)"\]|\[\s*Display\s*\(\s*Name\s*=\s*"(\S+)"\s*\)\s*\])/;

            all_files.forEach((value) => {

                fs.readFile(value,'utf8', (err, data) => {
                    if (err) {
                        console.error(err);
                        return;
                    }
                    data.match(/Localizer\["(\S+)"\]/g).forEach(value=>{
                        all_names.push( value[1] );
                    })
                    data.match(/_localizer\["(\S+)"\]/g).forEach(value=>{
                        all_names.push( value[1] );
                    })
                    data.match(/\[\s*Display\s*\(\s*Name\s*=\s*"(\S+)"\s*\)\s*\]/g).forEach(value=>{
                        all_names.push( value[1] );
                    })

                })
            });

            console.log(all_names);

            rl.close();
        });

});
