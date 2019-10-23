#import <UIKit/UIKit.h>

@interface FilePicker : NSObject<UIDocumentPickerDelegate>

@property(nonatomic) UIDocumentPickerViewController* pickerController;

+ (instancetype)sharedInstance;

- (void)show;

@end
