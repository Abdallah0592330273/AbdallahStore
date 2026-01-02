import os
import re

def remove_comments(text):
    def replacer(match):
        s = match.group(0)
        if s.startswith('/'):
            return " " # replace comment with a single space
        else:
            return s # it's a string literal, return it unchanged
    
    # Regex pattern:
    # Group 1: String literals (double or single quoted)
    # Group 3: Comments (multi-line /* ... */ or single-line // ...)
    pattern = re.compile(
        r'(("[^"\\]*(\\.[^"\\]*)*")|(\'[^\'\\]*(\\.[^\'\\]*)*\'))|(/\*[\s\S]*?\*/|//.*)',
        re.DOTALL | re.MULTILINE
    )
    return re.sub(pattern, replacer, text)

# Using raw string for path to avoid escape issues
directory = r"c:/Users/acc/Desktop/New Folder"

print(f"Scanning {directory}...")
try:
    for root, dirs, files in os.walk(directory):
        # Skip .git, .vs, bin, obj folders to be safe
        if '.git' in dirs: dirs.remove('.git')
        if '.vs' in dirs: dirs.remove('.vs')
        if 'bin' in dirs: dirs.remove('bin')
        if 'obj' in dirs: dirs.remove('obj')

        for file in files:
            if file.endswith(".cs"):
                filepath = os.path.join(root, file)
                try:
                    with open(filepath, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    new_content = remove_comments(content)
                    
                    if len(new_content) < len(content) * 0.1 and len(content) > 100:
                        print(f"Warning: Suspiciously large removal in {file}. Skipping.")
                        continue

                    if content != new_content:
                        with open(filepath, 'w', encoding='utf-8') as f:
                            f.write(new_content)
                        print(f"Cleaned {file}")
                except Exception as e:
                    print(f"Error processing {file}: {e}")
except Exception as e:
    print(f"Fatal error: {e}")
