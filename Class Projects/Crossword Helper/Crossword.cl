(defstruct trie
  (word nil) (count 0) (branches nil))

(defun subtrie (trie &rest letters)
   (traverse-trie trie letters 'read))

(defun read-words (words trie)
  (with-open-file (stream words)
    (do ((l (read-line stream nil 'eof) (read-line stream nil 'eof)))
        ((eq l 'eof) trie)
      (add-word l trie))))

(defun traverse-trie (trie word action)
  (let ((letters (coerce word 'list)))
    (if (null (car letters))
       trie
     (let* ((letter (coerce (car letters) 'character))
            (matched-trie (branch-matches trie letter)))
       (cond 
        ((and (eql action 'add)
              (null matched-trie))
         (incf (trie-count trie))
         (attach-branch trie letter)
         (traverse-trie trie letters action)) 
        (matched-trie
         (traverse-trie (cdr matched-trie) (cdr letters) action))
        (t nil))))))

(defun branch-matches (trie letter)
  (assoc-if #'(lambda (branch)
                (char-equal letter branch))
            (trie-branches trie)))

(defun add-word (word trie)
  (let ((word-branch (traverse-trie trie word 'add)))
    (setf (trie-word word-branch) word)))

(defun attach-branch (trie letter)
  (setf (trie-branches trie) ;;attach new branch
    (acons letter (make-trie) (trie-branches trie))))

(defun mapc-trie (fn trie)
  (dolist (branch (trie-branches trie))
    (funcall fn (car branch))))

(defun new-trie (&rest words)
  (add-words words (make-trie)))

(defun add-words (words trie)
  (dolist (word words trie)
    (add-word word trie)))

(define-test pattern-words
    (let ((trie (new-trie "abc" "abd" "aec" "bdc")))
      (assert-equal '("aec") (pattern-words "aec" trie))
      (assert-equal '("abc" "abd") (pattern-words "ab?" trie))
      (assert-equal '("abc" "aec") (pattern-words "a?c" trie))
      (assert-equal '("abc" "aec" "bdc") (pattern-words "??c" trie))
      (assert-equal '("abc" "abd" "aec" "bdc") (pattern-words "???" trie))))

(defun pattern-words (pattern trie)
  (reverse (search-trie pattern trie)))
      
(defun search-trie (word trie)
  (let ((letters (coerce word 'list)))
    (if (and (null letters) (trie-word trie))
        (list (trie-word trie))
      (mapcan #'(lambda (branch)
                  (search-trie (cdr letters) (cdr branch)))
        (get-matching-branches trie (car letters))))))

(defun get-matching-branches (trie letter)
  (remove-if-not #'(lambda (branch-letter)
                     (or (eql letter (car branch-letter))
                         (eql letter #\?)))
            (trie-branches trie)))

   
   
   
   
   
   
   
   
   
   
   
   
   
   
   