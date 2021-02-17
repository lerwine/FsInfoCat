sudo mkdir /mnt/testazureshare
if [ ! -d "/etc/smbcredentials" ]; then
sudo mkdir /etc/smbcredentials
fi
if [ ! -f "/etc/smbcredentials/servicenowdiag479.cred" ]; then
    sudo bash -c 'echo "username=servicenowdiag479" >> /etc/smbcredentials/servicenowdiag479.cred'
    sudo bash -c 'echo "password=jFpbf9ilT+uDN1sQYY6ClGXzrX7xjFwSd8nmg1AIMCA7AzDadASW51CBKVfcpivqf0cvFP7Yjq0ER/fyxZ25KQ==" >> /etc/smbcredentials/servicenowdiag479.cred'
fi
sudo chmod 600 /etc/smbcredentials/servicenowdiag479.cred

sudo bash -c 'echo "//servicenowdiag479.file.core.windows.net/testazureshare /mnt/testazureshare cifs nofail,vers=3.0,credentials=/etc/smbcredentials/servicenowdiag479.cred,dir_mode=0777,file_mode=0777,serverino" >> /etc/fstab'
sudo mount -t cifs //servicenowdiag479.file.core.windows.net/testazureshare /mnt/testazureshare -o vers=3.0,credentials=/etc/smbcredentials/servicenowdiag479.cred,dir_mode=0777,file_mode=0777,serverino