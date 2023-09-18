cd "$(dirname "$PWD")"


if [[ -n $(git status -s) ]]; then
    echo "Changes found. Pushing changes..."
	read -e -t 35 -p "Commit: " MSG
	message=''
	if [[ -z $MSG ]]; then
		message="update $(date +%Y/%m/%d-%H:%M:%S) "
	else
		message= $MSG
	fi
	
    git add -A && git commit -m "$message" && git push --set-upstream origin updater
else
    echo "No changes found. Skip pushing."
fi