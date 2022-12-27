#import <Foundation/Foundation.h>
#import "MSACExceptionModel.h"

@interface MSACWrapperExceptionModel : MSACExceptionModel

@property(nonatomic) NSArray<MSACWrapperExceptionModel *> *innerExceptions;
@property(nonatomic, copy) NSString *wrapperSdkName;

@end
