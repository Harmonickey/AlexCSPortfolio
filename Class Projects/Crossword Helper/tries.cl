(defparameter *word-file* 
  (merge-pathnames "C:/Users/Alex/Artificial Intelligence/crosswd.txt"
                   (or *compile-file-truename* *load-truename*)))

(defparameter *word-file-empty*
  (merge-pathnames "C:/Users/Alex/Desktop/empty.txt"
                   (or *compile-file-truename* *load-truename*)))

(in-package :trie)

(defpackage trie
  (:use :common-lisp lisp-unit)
  (:export :trie :make-trie :add-word :add-word-with-offset :subtrie :trie-word :trie-count :trie-count-helper :read-words))

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

(define-test subtrie
    (assert-false (subtrie (make-trie) #\a))
  (let ((trie (new-trie "abc" "def")))
    (assert-true (subtrie trie #\a))
    (assert-true (subtrie trie #\d))
    (assert-false (subtrie trie #\b))
    (assert-true (subtrie trie #\a #\b))
    (assert-eq trie (subtrie trie))
    ))

(define-test trie-word
    (assert-false (trie-word (make-trie)))
  (let ((trie (new-trie "abc" "def")))
    (assert-false (trie-word trie))
    (assert-false (trie-word (subtrie trie #\a)))
    (assert-false (trie-word (subtrie trie #\a #\b)))
    (assert-equal "abc" (trie-word (subtrie trie #\a #\b #\c)))
    ))

(define-test trie-count
    (assert-equal 0 (trie-count (make-trie)))
  (assert-equal 1 (trie-count (new-trie "abc")))
  (let ((trie (new-trie "ab" "abc" "abd" "b")))
    (assert-equal 4 (trie-count trie))
    (assert-equal 3 (trie-count (subtrie trie #\a)))
    (assert-equal 3 (trie-count (subtrie trie #\a #\b)))
    (assert-equal 1 (trie-count (subtrie trie #\a #\b #\c)))
    (assert-equal 1 (trie-count (subtrie trie #\a #\b #\d)))
    ))(define-test trie-word
          (assert-false (trie-word (make-trie)))
        (let ((trie (new-trie "abc" "def")))
          (assert-false (trie-word trie))
          (assert-false (trie-word (subtrie trie #\a)))
          (assert-false (trie-word (subtrie trie #\a #\b)))
          (assert-equal "abc" (trie-word (subtrie trie #\a #\b #\c)))
          ))